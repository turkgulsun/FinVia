using System.Text;
using Finvia.Shared.Outbox.Abstractions;
using Finvia.Shared.Outbox.Enums;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;

namespace KycService.Worker;

public class Worker : BackgroundService
{
    private readonly IServiceProvider _provider;
    private readonly ILogger<Worker> _logger;
    private readonly IModel _channel;
    private const int MaxRetryCount = 3;

    public Worker(IServiceProvider provider, ILogger<Worker> logger, IModel channel)
    {
        _provider = provider;
        _logger = logger;
        _channel = channel;

        _channel.ExchangeDeclare("event_bus", ExchangeType.Topic, durable: true);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _provider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<IOutboxDbContext>();

            var pending = await db.OutboxMessages
                .Where(x =>
                    (x.Status == OutboxStatus.Pending) ||
                    (x.Status == OutboxStatus.Failed && x.RetryCount < MaxRetryCount))
                .OrderBy(x => x.CreatedAt)
                .Take(10)
                .ToListAsync(stoppingToken);

            foreach (var message in pending)
            {
                try
                {
                    message.Status = OutboxStatus.Retrying;
                    message.RetryCount++;

                    var props = _channel.CreateBasicProperties();
                    props.ContentType = "application/json";
                    props.DeliveryMode = 2;
                    props.Headers = new Dictionary<string, object>
                    {
                        ["correlationId"] = message.CorrelationId.ToString()
                    };

                    _channel.BasicPublish(
                        exchange: "event_bus",
                        routingKey: message.EventType.ToLowerInvariant(),
                        basicProperties: props,
                        body: Encoding.UTF8.GetBytes(message.Payload)
                    );

                    message.Status = OutboxStatus.Processed;
                    message.ProcessedAt = DateTime.UtcNow;

                    _logger.LogInformation("✅ Published: {EventType} RetryCount: {Retry}", message.EventType, message.RetryCount);
                }
                catch (Exception ex)
                {
                    if (message.RetryCount >= MaxRetryCount)
                    {
                        message.Status = OutboxStatus.DeadLetter;
                        message.FailedAt = DateTime.UtcNow;
                        _logger.LogWarning("❌ Moved to DeadLetter: {EventType}", message.EventType);
                    }
                    else
                    {
                        message.Status = OutboxStatus.Failed;
                        _logger.LogError(ex, "❌ Failed to publish: {EventType} RetryCount: {Retry}", message.EventType, message.RetryCount);
                    }
                }
            }

            await db.SaveChangesAsync(stoppingToken);
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }
}

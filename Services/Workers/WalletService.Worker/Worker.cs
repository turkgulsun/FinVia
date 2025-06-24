using System.Text;
using System.Text.Json;
using Finvia.Shared.IntegrationEvents.Wallet;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace WalletService.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IModel _channel;

    public Worker(ILogger<Worker> logger, IModel channel)
    {
        _logger = logger;
        _channel = channel;

        _channel.ExchangeDeclare("event_bus", ExchangeType.Topic, durable: true);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var queueName = _channel.QueueDeclare().QueueName;
        _channel.QueueBind(queue: queueName, exchange: "event_bus", routingKey: "wallet.reward");

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (_, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var correlationId = ea.BasicProperties.Headers["correlationId"]?.ToString() ?? "-";

            var @event = JsonSerializer.Deserialize<RewardWalletIntegrationEvent>(message);

            _logger.LogInformation("🎁 Received Wallet Reward for User: {UserId}, Amount: {Amount} {Currency}, CorrelationId: {CorrelationId}",
                @event?.UserId, @event?.Amount, @event?.Currency, correlationId);

            // TODO: Wallet güncellemesi burada yapılacak
        };

        _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

        return Task.CompletedTask;
    }
}
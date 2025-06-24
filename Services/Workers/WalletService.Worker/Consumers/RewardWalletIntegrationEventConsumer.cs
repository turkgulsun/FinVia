using System.Text;
using System.Text.Json;
using Finvia.Shared.IntegrationEvents.Wallet;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using WalletService.Worker.Persistence;
using WalletService.Worker.Persistence.Entities;
using WalletService.Worker.Persistence.Enums;

namespace WalletService.Worker.Consumers;

public class RewardWalletIntegrationEventConsumer : BackgroundService
{
    private readonly ILogger<RewardWalletIntegrationEventConsumer> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IModel _channel;

    public RewardWalletIntegrationEventConsumer(
        ILogger<RewardWalletIntegrationEventConsumer> logger,
        IServiceProvider serviceProvider,
        IModel channel)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _channel = channel;

        _channel.ExchangeDeclare("event_bus", ExchangeType.Topic, durable: true);
        _channel.QueueDeclare("wallet.reward.queue", durable: true, exclusive: false);
        _channel.QueueBind("wallet.reward.queue", "event_bus", routingKey: "reward.wallet");
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (_, ea) =>
        {
            try
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                var integrationEvent = JsonSerializer.Deserialize<RewardWalletIntegrationEvent>(message);

                if (integrationEvent == null)
                {
                    _logger.LogWarning("❌ Invalid message received - deserialization failed.");
                    return;
                }

                _logger.LogInformation("📩 Received reward event for UserId: {UserId}, Amount: {Amount} {Currency}",
                    integrationEvent.UserId, integrationEvent.Amount, integrationEvent.Currency);

                using var scope = _serviceProvider.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<WalletDbContext>();

                var wallet = await db.Wallets.FindAsync(new object[] { integrationEvent.UserId }, stoppingToken);
                if (wallet is null)
                {
                    _logger.LogWarning("⚠️ Wallet not found for UserId: {UserId}", integrationEvent.UserId);
                    return;
                }

                if (!wallet.IsActive)
                {
                    _logger.LogWarning("⛔ Cannot credit inactive wallet for UserId: {UserId}", integrationEvent.UserId);
                    return;
                }

                wallet.Credit(integrationEvent.Amount);

                var transaction = new WalletTransaction
                {
                    Id = Guid.NewGuid(),
                    WalletId = wallet.Id,
                    Amount = integrationEvent.Amount,
                    Currency = integrationEvent.Currency,
                    Type = TransactionType.CampaignReward,
                    Description = "KYC reward campaign",
                    CorrelationId = integrationEvent.CorrelationId,
                    CreatedAt = DateTime.UtcNow
                };

                db.WalletTransactions.Add(transaction);
                await db.SaveChangesAsync(stoppingToken);

                _logger.LogInformation("💰 Wallet credited and transaction saved for UserId: {UserId}", integrationEvent.UserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ An error occurred while handling RewardWalletIntegrationEvent.");
            }
        };

        _channel.BasicConsume(queue: "wallet.reward.queue", autoAck: true, consumer: consumer);
        return Task.CompletedTask;
    }
}

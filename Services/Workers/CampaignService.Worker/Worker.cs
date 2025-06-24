using System.Text;
using System.Text.Json;
using CampaignService.Worker.Models;
using CampaignService.Worker.Persistence;
using Finvia.Shared.IntegrationEvents.Kyc;
using Finvia.Shared.IntegrationEvents.Wallet;
using Finvia.Shared.Outbox.Abstractions;
using Finvia.Shared.Outbox.Enums;
using Finvia.Shared.Outbox.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CampaignService.Worker;

public class Worker(ILogger<Worker> logger, IModel channel, IServiceProvider serviceProvider) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        channel.ExchangeDeclare("event_bus", ExchangeType.Topic, durable: true);

        var queueName = channel.QueueDeclare().QueueName;
        channel.QueueBind(queue: queueName, exchange: "event_bus", routingKey: "kyc.approved");

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var correlationId = ea.BasicProperties.Headers?["correlationId"]?.ToString() ?? "-";

            var @event = JsonSerializer.Deserialize<KycApprovedIntegrationEvent>(message);

            logger.LogInformation("📥 Received KYC Approved Event for UserId: {UserId}, CorrelationId: {CorrelationId}",
                @event?.UserId, correlationId);

            if (@event == null)
            {
                logger.LogWarning("❌ Event deserialization failed.");
                return;
            }

            using var scope = serviceProvider.CreateScope();
            var campaignDb = scope.ServiceProvider.GetRequiredService<CampaignDbContext>();
            var outboxDb = scope.ServiceProvider.GetRequiredService<IOutboxDbContext>();

            var reward = new CampaignReward
            {
                UserId = @event.UserId,
                Amount = 100,
                Currency = "USD",
                CreatedAt = DateTime.UtcNow,
                CorrelationId = @event.CorrelationId
            };

            await campaignDb.CampaignRewards.AddAsync(reward, stoppingToken);
            await campaignDb.SaveChangesAsync(stoppingToken);

            var rewardEvent = new RewardWalletIntegrationEvent(
                userId: @event.UserId,
                amount: 100,
                currency: "USD",
                source: "Campaign",
                description: "KYC Approval Bonus",
                correlationId: @event.CorrelationId
            );

            var outboxMessage = new OutboxMessage
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                EventType = nameof(RewardWalletIntegrationEvent),
                Payload = JsonSerializer.Serialize(rewardEvent),
                CorrelationId = rewardEvent.CorrelationId,
                Status = OutboxStatus.Pending
            };

            await outboxDb.OutboxMessages.AddAsync(outboxMessage, stoppingToken);
            await outboxDb.SaveChangesAsync(stoppingToken);

            logger.LogInformation("✅ Reward created and outbox message written for UserId: {UserId}", @event.UserId);
        };

        channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

        return Task.CompletedTask;
    }
}
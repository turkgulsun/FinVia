using System.Text.Json;
using Finvia.Shared.Messaging.Abstractions;
using Finvia.Shared.Outbox.Abstractions;
using Finvia.Shared.Outbox.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Finvia.Shared.Outbox;

public class OutboxMessagePublisher(IOutboxDbContext dbContext, IEventBus eventBus,
    ILogger<OutboxMessagePublisher> logger) : IOutboxMessagePublisher
{
    public async Task PublishPendingMessagesAsync(CancellationToken cancellationToken)
    {
        var messages = await dbContext.OutboxMessages
            .Where(m => m.Status == OutboxStatus.Pending || m.Status == OutboxStatus.Retrying)
            .OrderBy(m => m.CreatedAt)
            .Take(10)
            .ToListAsync(cancellationToken);

        foreach (var message in messages)
        {
            try
            {
                var type = Type.GetType(message.EventType);
                if (type == null)
                {
                    logger.LogError("Outbox message type bulunamadı: {Type}", message.EventType);
                    continue;
                }

                var @event = JsonSerializer.Deserialize(message.Payload, type);

                await eventBus.PublishAsync((dynamic)@event!, message.EventType, message.CorrelationId);

                message.MarkAsProcessed(); // extension veya entity içinde metod
            }
            catch (Exception ex)
            {
                message.IncrementRetry(); // extension veya entity içinde metod
                logger.LogError(ex, "Outbox mesajı publish edilirken hata oluştu. Id: {Id}", message.Id);
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
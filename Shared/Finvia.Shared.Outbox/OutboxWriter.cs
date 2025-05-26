using System.Text.Json;
using Finvia.Shared.Outbox.Abstractions;
using Finvia.Shared.Outbox.Models;

namespace Finvia.Shared.Outbox;

public class OutboxWriter(IOutboxDbContext context) : IOutboxWriter
{
    public async Task SaveAsync<T>(T @event, string eventType, Guid correlationId, CancellationToken cancellationToken = default) where T : class
    {
        var payload = JsonSerializer.Serialize(@event);
        var message = new OutboxMessage
        {
            EventType = eventType,
            Payload = payload,
            CorrelationId = correlationId
        };

        context.OutboxMessages.Add(message);
        await context.SaveChangesAsync(cancellationToken);
    }
}
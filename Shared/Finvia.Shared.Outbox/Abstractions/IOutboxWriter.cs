namespace Finvia.Shared.Outbox.Abstractions;

public interface IOutboxWriter
{
    Task SaveAsync<T>(T @event, string eventType, Guid correlationId, CancellationToken cancellationToken = default) where T : class;
}
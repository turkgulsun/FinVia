namespace Finvia.Shared.Outbox.Abstractions;

public interface IOutboxMessagePublisher
{
    Task PublishPendingMessagesAsync(CancellationToken cancellationToken);
}
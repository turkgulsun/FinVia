namespace Finvia.Shared.Outbox.Abstractions;

public interface IIntegrationEventDispatcher
{
    Task DispatchAsync<T>(T @event, CancellationToken cancellationToken = default) where T : class;
}
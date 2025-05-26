namespace Finvia.Shared.Messaging.Abstractions;

public interface IEventBus
{
    Task PublishAsync<T>(T @event, string routingKey, Guid correlationId) where T : class;
}
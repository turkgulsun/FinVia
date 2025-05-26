namespace Finvia.Shared.IntegrationEvents.Abstractions;

public interface IIntegrationEvent
{
    Guid CorrelationId { get; }
}
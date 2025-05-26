using Finvia.Shared.IntegrationEvents.Abstractions;

namespace Finvia.Shared.IntegrationEvents.Kyc;

public sealed class KycFailedIntegrationEvent : IIntegrationEvent
{
    public Guid UserId { get; init; }
    public DateTime FailedAt { get; init; }
    public Guid CorrelationId { get; init; }

    public string Source { get; init; } = "KycService";

    public KycFailedIntegrationEvent(Guid userId, DateTime failedAt, Guid correlationId)
    {
        UserId = userId;
        FailedAt = failedAt;
        CorrelationId = correlationId;
    }
}

using Finvia.Shared.IntegrationEvents.Abstractions;

namespace Finvia.Shared.IntegrationEvents.Kyc;

public sealed class KycApprovedIntegrationEvent : IIntegrationEvent
{
    public Guid UserId { get; init; }
    public DateTime ApprovedAt { get; init; }
    public Guid CorrelationId { get; init; }

    public string Source { get; init; } = "KycService";

    public KycApprovedIntegrationEvent(Guid userId, DateTime approvedAt, Guid correlationId)
    {
        UserId = userId;
        ApprovedAt = approvedAt;
        CorrelationId = correlationId;
    }
}

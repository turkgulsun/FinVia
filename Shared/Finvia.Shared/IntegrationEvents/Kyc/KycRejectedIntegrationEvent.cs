using Finvia.Shared.IntegrationEvents.Abstractions;

namespace Finvia.Shared.IntegrationEvents.Kyc;

public sealed class KycRejectedIntegrationEvent : IIntegrationEvent
{
    public Guid UserId { get; init; }
    public DateTime RejectedAt { get; init; }
    public Guid CorrelationId { get; init; }

    public string Source { get; init; } = "KycService";

    public KycRejectedIntegrationEvent(Guid userId, DateTime rejectedAt, Guid correlationId)
    {
        UserId = userId;
        RejectedAt = rejectedAt;
        CorrelationId = correlationId;
    }
}

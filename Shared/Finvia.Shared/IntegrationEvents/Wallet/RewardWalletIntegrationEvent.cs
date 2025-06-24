using Finvia.Shared.IntegrationEvents.Abstractions;

namespace Finvia.Shared.IntegrationEvents.Wallet;

/// Event published when a user earns a reward from a campaign and their wallet needs to be credited.
/// </summary>
public sealed class RewardWalletIntegrationEvent : IIntegrationEvent
{
    public Guid UserId { get; init; }
    public decimal Amount { get; init; }
    public string Currency { get; init; }
    public string Source { get; init; } = "Campaign";
    public string? Description { get; init; }
    public Guid CorrelationId { get; init; }

    public RewardWalletIntegrationEvent(Guid userId, decimal amount, string currency, string source, string? description, Guid correlationId)
    {
        UserId = userId;
        Amount = amount;
        Currency = currency;
        Source = source;
        Description = description;
        CorrelationId = correlationId;
    }
}
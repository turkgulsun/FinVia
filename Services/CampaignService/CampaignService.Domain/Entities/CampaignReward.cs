using Finvia.Shared.Domain;

namespace CampaignService.Domain.Entities;

public class CampaignReward : Entity, IAggregateRoot
{
    public Guid UserId { get; private set; }

    public decimal Amount { get; private set; }

    public string Currency { get; private set; }

    public Guid CorrelationId { get; private set; }

    public CampaignReward(Guid userId, decimal amount, string currency, Guid correlationId)
    {
        UserId = userId;
        Amount = amount;
        Currency = currency;
        CorrelationId = correlationId;
        CreatedAt = DateTime.UtcNow;
    }

    public DateTime CreatedAt { get; private set; }
}
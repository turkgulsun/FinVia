using Finvia.Shared.Domain;

namespace CampaignService.Worker.Models;

public class CampaignReward: Entity, IAggregateRoot
{
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = default!;
    public Guid CorrelationId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
}
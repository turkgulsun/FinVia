using Finvia.Shared.Domain;
using MediatR;

namespace KycService.Domain.Events;

public class KycApprovedEvent : IDomainEvent, INotification
{
    public Guid UserId { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public KycApprovedEvent(Guid userId)
    {
        UserId = userId;
    }
}
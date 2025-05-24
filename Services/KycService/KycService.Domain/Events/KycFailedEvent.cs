using Finvia.Shared.Domain;
using MediatR;

namespace KycService.Domain.Events;

public class KycFailedEvent : IDomainEvent, INotification
{
    public Guid UserId { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public KycFailedEvent(Guid userId)
    {
        UserId = userId;
    }
}
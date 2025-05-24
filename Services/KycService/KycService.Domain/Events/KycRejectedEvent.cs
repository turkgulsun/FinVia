using Finvia.Shared.Domain;
using MediatR;

namespace KycService.Domain.Events;

public class KycRejectedEvent : IDomainEvent, INotification
{
    public Guid UserId { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public KycRejectedEvent(Guid userId)
    {
        UserId = userId;
    }
}
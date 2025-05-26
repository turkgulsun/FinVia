using Finvia.Shared.IntegrationEvents.Kyc;
using Finvia.Shared.Outbox.Abstractions;
using KycService.Application.Abstractions;
using KycService.Domain.Enums;
using KycService.Domain.Events;
using MediatR;

namespace KycService.Infrastructure.EventHandlers;

public class KycRejectedEventHandler
    (IKycAuditService audit, IIntegrationEventDispatcher eventDispatcher) : INotificationHandler<KycRejectedEvent>
{
    public async Task Handle(KycRejectedEvent notification, CancellationToken cancellationToken)
    {
        await audit.LogAsync(notification.UserId, KycStatus.Rejected, "KYC reject.", cancellationToken);

        var integrationEvent = new KycRejectedIntegrationEvent(
            notification.UserId,
            DateTime.UtcNow,
            correlationId: Guid.NewGuid()
        );

        await eventDispatcher.DispatchAsync(integrationEvent, cancellationToken);
    }
    
}
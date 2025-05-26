using Finvia.Shared.IntegrationEvents.Kyc;
using Finvia.Shared.Outbox.Abstractions;
using KycService.Application.Abstractions;
using KycService.Domain.Enums;
using KycService.Domain.Events;
using MediatR;

namespace KycService.Infrastructure.EventHandlers;

public class KycFailedEventHandler
    (IKycAuditService audit, IIntegrationEventDispatcher eventDispatcher) : INotificationHandler<KycFailedEvent>
{
    public async Task Handle(KycFailedEvent notification, CancellationToken cancellationToken)
    {
        await audit.LogAsync(notification.UserId, KycStatus.Failed, "KYC failed.", cancellationToken);

        var integrationEvent = new KycFailedIntegrationEvent(
            notification.UserId,
            DateTime.UtcNow,
            correlationId: Guid.NewGuid()
        );

        await eventDispatcher.DispatchAsync(integrationEvent, cancellationToken);
    }
}
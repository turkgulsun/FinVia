using Finvia.Shared.IntegrationEvents.Kyc;
using Finvia.Shared.Outbox.Abstractions;
using KycService.Application.Abstractions;
using KycService.Domain.Enums;
using KycService.Domain.Events;
using MediatR;

namespace KycService.Infrastructure.EventHandlers;

public class KycApprovedEventHandler(IKycAuditService audit, IIntegrationEventDispatcher eventDispatcher) : INotificationHandler<KycApprovedEvent>
{
    public async Task Handle(KycApprovedEvent notification, CancellationToken cancellationToken)
    {
        await audit.LogAsync(notification.UserId, KycStatus.Approved, "KYC approved.", cancellationToken);

        var integrationEvent = new KycApprovedIntegrationEvent(
            notification.UserId,
            DateTime.UtcNow,
            correlationId: Guid.NewGuid()
        );

        await eventDispatcher.DispatchAsync(integrationEvent, cancellationToken);
    }
}

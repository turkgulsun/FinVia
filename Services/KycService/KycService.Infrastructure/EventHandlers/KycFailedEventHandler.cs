using KycService.Domain.Entities;
using KycService.Domain.Enums;
using KycService.Domain.Events;
using KycService.Infrastructure.Persistence;
using MediatR;

namespace KycService.Infrastructure.EventHandlers;

public class KycFailedEventHandler(KycDbContext db) : INotificationHandler<KycFailedEvent>
{
    public async Task Handle(KycFailedEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"[EVENT] ⚠️ KYC Failed for UserId: {notification.UserId} at {notification.OccurredOn}");

        var log = new KycAuditLog(
            notification.UserId,
            KycStatus.Failed,
            "KYC failed."
        );

        db.AuditLogs.Add(log);
        await db.SaveChangesAsync(cancellationToken);
    }
}
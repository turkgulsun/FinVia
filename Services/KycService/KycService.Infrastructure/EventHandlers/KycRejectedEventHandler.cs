using KycService.Domain.Entities;
using KycService.Domain.Enums;
using KycService.Domain.Events;
using KycService.Infrastructure.Persistence;
using MediatR;

namespace KycService.Infrastructure.EventHandlers;

public class KycRejectedEventHandler(KycDbContext db) : INotificationHandler<KycRejectedEvent>
{
    public async Task Handle(KycRejectedEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"[EVENT] ❌ KYC Rejected for UserId: {notification.UserId} at {notification.OccurredOn}");

        var log = new KycAuditLog(
            notification.UserId,
            KycStatus.Rejected,
            "KYC rejected."
        );

        db.AuditLogs.Add(log);
        await db.SaveChangesAsync(cancellationToken);
    }
}
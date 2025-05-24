using KycService.Domain.Entities;
using KycService.Domain.Enums;
using KycService.Domain.Events;
using KycService.Infrastructure.Persistence;
using MediatR;

namespace KycService.Infrastructure.EventHandlers;

public class KycApprovedEventHandler(KycDbContext db) : INotificationHandler<KycApprovedEvent>
{
    public async Task Handle(KycApprovedEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"[EVENT] ✅ KYC Approved for UserId: {notification.UserId} at {notification.OccurredOn}");

        var log = new KycAuditLog(
            notification.UserId,
            KycStatus.Approved,
            "KYC approved."
        );

        db.AuditLogs.Add(log);
        await db.SaveChangesAsync(cancellationToken);
    }
}
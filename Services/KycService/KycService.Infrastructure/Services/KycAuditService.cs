using KycService.Application.Abstractions;
using KycService.Domain.Entities;
using KycService.Domain.Enums;
using KycService.Infrastructure.Persistence;

namespace KycService.Infrastructure.Services;

public class KycAuditService(KycDbContext db) : IKycAuditService
{
    public async Task LogAsync(Guid userId, KycStatus status, string message, CancellationToken cancellationToken)
    {
        var log = new KycAuditLog(userId, status, message);
        db.AuditLogs.Add(log);
        await db.SaveChangesAsync(cancellationToken);
    }
}
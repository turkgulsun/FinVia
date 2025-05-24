using Finvia.Shared.Domain;
using KycService.Domain.Enums;

namespace KycService.Domain.Entities;

public class KycAuditLog : Entity
{
    public Guid UserId { get; private set; }
    public KycStatus Status { get; private set; }
    public string Message { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private KycAuditLog()
    {
    }

    public KycAuditLog(Guid userId, KycStatus status, string message)
    {
        UserId = userId;
        Status = status;
        Message = message;
        CreatedAt = DateTime.UtcNow;
    }
}
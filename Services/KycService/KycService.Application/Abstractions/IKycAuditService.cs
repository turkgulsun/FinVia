using KycService.Domain.Enums;

namespace KycService.Application.Abstractions;

public interface IKycAuditService
{
    Task LogAsync(Guid userId, KycStatus status, string message, CancellationToken cancellationToken);
}
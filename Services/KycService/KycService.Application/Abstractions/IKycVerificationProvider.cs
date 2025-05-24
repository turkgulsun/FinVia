using Finvia.Shared.Application.Enums;

namespace KycService.Application.Abstractions;

public interface IKycVerificationProvider
{
    Task<KycVerificationResult> VerifyAsync(Guid userId);
}
using Finvia.Shared.Application.Enums;
using Finvia.Shared.Common;
using KycService.Application.Abstractions;
using KycService.Application.Common;
using KycService.Domain.Abstractions;
using KycService.Domain.Enums;
using MediatR;

namespace KycService.Application.Commands.VerifyKyc;

public class VerifyKycCommandHandler(IKycRepository kycRepository, IKycVerificationProvider provider,
    DomainEventDispatcher dispatcher) : IRequestHandler<VerifyKycCommand, Result<KycStatus>>
{
    public async Task<Result<KycStatus>> Handle(VerifyKycCommand request, CancellationToken cancellationToken)
    {
        var kyc = await kycRepository.GetByUserIdAsync(request.UserId);
        if (kyc is null)
            return Result<KycStatus>.Failure("KYC record not found");

        var result = await provider.VerifyAsync(request.UserId);

        switch (result)
        {
            case KycVerificationResult.Approved:
                kyc.Approve();
                break;
            case KycVerificationResult.Rejected:
                kyc.Reject();
                break;
            case KycVerificationResult.Failed:
                kyc.Fail();
                break;
        }

        await kycRepository.UpdateAsync(kyc);

        await dispatcher.DispatchAsync(kyc.DomainEvents);
        kyc.ClearDomainEvents();

        return Result<KycStatus>.Success(kyc.Status, $"KYC {kyc.Status}");
    }
}
using Finvia.Shared.Common;
using KycService.Domain.Abstractions;
using KycService.Domain.Enums;
using MediatR;

namespace KycService.Application.Queries.GetKycStatus;

public class GetKycStatusQueryHandler(IKycRepository kycRepository) : IRequestHandler<GetKycStatusQuery, Result<KycStatus>>
{
    public async Task<Result<KycStatus>> Handle(GetKycStatusQuery request, CancellationToken cancellationToken)
    {
        var kyc = await kycRepository.GetByUserIdAsync(request.UserId);

        if (kyc is null)
            return Result<KycStatus>.Failure("KYC record not found");

        return Result<KycStatus>.Success(kyc.Status);
    }
}

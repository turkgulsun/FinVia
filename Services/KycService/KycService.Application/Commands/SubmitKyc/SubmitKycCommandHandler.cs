using Finvia.Shared.Common;
using KycService.Application.Commands.SubmitKyc;
using KycService.Domain.Abstractions;
using KycService.Domain.Messages;
using MediatR;

namespace KycService.Application.Commands.Kyc;

public class SubmitKycCommandHandler(IKycRepository kycRepository) : IRequestHandler<SubmitKycCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(SubmitKycCommand request, CancellationToken cancellationToken)
    {
        var existing = await kycRepository.GetByUserIdAsync(request.UserId);
        if (existing is not null)
            return Result<Guid>.Failure("KYC already submitted.");

        var kyc = Domain.Entities.Kyc.Create(
            request.UserId,
            request.FirstName,
            request.LastName,
            request.NationalId,
            request.DateOfBirth,
            request.CountryCode
        );

        await kycRepository.AddAsync(kyc);

        return Result<Guid>.Success(kyc.Id, GeneralMessages.KycRegistered);
    }
}
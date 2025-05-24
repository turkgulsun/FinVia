using Finvia.Shared.Common;
using MediatR;

namespace KycService.Application.Commands.SubmitKyc;

public record SubmitKycCommand(
    Guid UserId,
    string FirstName,
    string LastName,
    string NationalId,
    DateTime DateOfBirth,
    string CountryCode
) : IRequest<Result<Guid>>;

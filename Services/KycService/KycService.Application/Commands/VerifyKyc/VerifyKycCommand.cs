using Finvia.Shared.Common;
using KycService.Domain.Enums;
using MediatR;

namespace KycService.Application.Commands.VerifyKyc;

public record VerifyKycCommand(Guid UserId) : IRequest<Result<KycStatus>>;

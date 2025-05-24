using Finvia.Shared.Common;
using KycService.Domain.Enums;
using MediatR;

namespace KycService.Application.Queries.GetKycStatus;

public record GetKycStatusQuery(Guid UserId) : IRequest<Result<KycStatus>>;

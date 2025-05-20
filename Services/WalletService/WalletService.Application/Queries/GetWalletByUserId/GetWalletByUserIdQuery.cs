using Finvia.Shared.Common;
using MediatR;
using WalletService.Application.DTOs;

namespace WalletService.Application.Queries.GetWalletByUserId;

public record GetWalletByUserIdQuery(Guid UserId) : IRequest<Result<WalletDto>>;
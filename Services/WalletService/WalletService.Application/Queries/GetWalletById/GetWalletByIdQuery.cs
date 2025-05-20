using Finvia.Shared.Common;
using MediatR;
using WalletService.Application.DTOs;

namespace WalletService.Application.Queries.GetWalletById;

public sealed record GetWalletByIdQuery(Guid WalletId) : IRequest<Result<WalletDto>>;

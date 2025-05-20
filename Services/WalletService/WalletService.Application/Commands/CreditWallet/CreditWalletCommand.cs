using Finvia.Shared.Common;
using MediatR;

namespace WalletService.Application.Commands.CreditWallet;

public record CreditWalletCommand(Guid WalletId, decimal Amount) : IRequest<Result<Guid>>;
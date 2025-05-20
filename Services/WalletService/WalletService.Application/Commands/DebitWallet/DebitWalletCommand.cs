using Finvia.Shared.Common;
using MediatR;

namespace WalletService.Application.Commands.DebitWallet;

public record DebitWalletCommand(Guid WalletId, decimal Amount) : IRequest<Result<Guid>>;
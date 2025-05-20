using Finvia.Shared.Common;
using MediatR;
using WalletService.Application.Abstractions;
using WalletService.Domain.Messages;

namespace WalletService.Application.Commands.DebitWallet;

public class DebitWalletCommandHandler(IWalletService walletService) : IRequestHandler<DebitWalletCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(DebitWalletCommand command, CancellationToken cancellationToken)
    {
        var wallet = await walletService.GetByIdAsync(command.WalletId);

        if (wallet is null)
            return Result<Guid>.Failure(WalletMessages.NotFound);

        if (!wallet.IsActive)
            return Result<Guid>.Failure(WalletMessages.WalletInactive);

        if (wallet.Balance.Amount < command.Amount)
            return Result<Guid>.Failure(WalletMessages.InsufficientFunds);

        wallet.Debit(command.Amount);

        await walletService.SaveAsync(wallet);

        return Result<Guid>.Success(wallet.Id.Value);
    }
}
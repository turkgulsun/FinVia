using Finvia.Shared.Common;
using MediatR;
using WalletService.Application.Abstractions;
using WalletService.Domain.Messages;

namespace WalletService.Application.Commands.CreditWallet;

public class CreditWalletCommandHandler(IWalletService walletService) : IRequestHandler<CreditWalletCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreditWalletCommand command, CancellationToken cancellationToken)
    {
        var wallet = await walletService.GetByIdAsync(command.WalletId);

        if (wallet == null)
            return Result<Guid>.Failure(WalletMessages.NotFound);

        if (!wallet.IsActive)
            return Result<Guid>.Failure(WalletMessages.WalletInactive);

        wallet.Credit(command.Amount);

        await walletService.SaveAsync(wallet);

        return Result<Guid>.Success(wallet.Id.Value);
    }
}
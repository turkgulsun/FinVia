using Finvia.Shared.Common;
using MediatR;
using WalletService.Domain.Abstractions;
using WalletService.Domain.Messages;

namespace WalletService.Application.Commands.CreditWallet;

public class CreditWalletCommandHandler(IWalletRepository walletRepository) : IRequestHandler<CreditWalletCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreditWalletCommand command, CancellationToken cancellationToken)
    {
        var wallet = await walletRepository.GetByIdAsync(command.WalletId);

        if (wallet == null)
            return Result<Guid>.Failure(WalletMessages.NotFound);

        if (!wallet.IsActive)
            return Result<Guid>.Failure(WalletMessages.WalletInactive);

        wallet.Credit(command.Amount);

        await walletRepository.SaveAsync(wallet);

        return Result<Guid>.Success(wallet.Id.Value);
    }
}
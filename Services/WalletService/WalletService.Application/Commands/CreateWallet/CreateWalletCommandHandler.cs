using Finvia.Shared.Common;
using MediatR;
using WalletService.Domain.Abstractions;
using WalletService.Domain.Entities;
using WalletService.Domain.Enums;
using WalletService.Domain.Messages;

namespace WalletService.Application.Commands.CreateWallet;

public class CreateWalletCommandHandler(IWalletRepository walletRepository) : IRequestHandler<CreateWalletCommand, Result<Guid>>
{

    public async Task<Result<Guid>> Handle(CreateWalletCommand command, CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<Currency>(command.Currency, out var currency))
            return Result<Guid>.Failure(WalletMessages.InvalidCurrency);

        var wallet = new Wallet(command.UserId, currency);

        await walletRepository.SaveAsync(wallet);

        return Result<Guid>.Success(wallet.Id.Value, WalletMessages.WalletCreated);
    }
}
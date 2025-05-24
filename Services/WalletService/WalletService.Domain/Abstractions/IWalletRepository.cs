using WalletService.Domain.Entities;

namespace WalletService.Domain.Abstractions;

public interface IWalletRepository
{
    Task<Wallet?> GetByIdAsync(Guid walletId);
    Task<Wallet?> GetByUserIdAsync(Guid userId);
    Task SaveAsync(Wallet wallet);
}
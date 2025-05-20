using WalletService.Domain.Entities;

namespace WalletService.Application.Abstractions;

public interface IWalletService
{
    Task<Wallet?> GetByIdAsync(Guid walletId);
    Task<Wallet?> GetByUserIdAsync(Guid userId);
    Task SaveAsync(Wallet wallet);
}
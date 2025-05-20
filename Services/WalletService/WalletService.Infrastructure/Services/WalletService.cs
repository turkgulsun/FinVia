using Microsoft.EntityFrameworkCore;
using WalletService.Application.Abstractions;
using WalletService.Domain.Entities;
using WalletService.Infrastructure.Persistence;

namespace WalletService.Infrastructure.Services;

public class WalletService(WalletDbContext context) : IWalletService
{
    public async Task<Wallet?> GetByIdAsync(Guid walletId)
    {
        return await context.Wallets.AsNoTracking()
            .FirstOrDefaultAsync(w => w.Id == walletId);
    }

    public async Task<Wallet?> GetByUserIdAsync(Guid userId)
    {
        return await context.Wallets.AsNoTracking()
            .FirstOrDefaultAsync(w => w.UserId == userId);
    }

    public async Task SaveAsync(Wallet wallet)
    {
        await context.Wallets.AddAsync(wallet);

        await context.SaveChangesAsync();
    }
}
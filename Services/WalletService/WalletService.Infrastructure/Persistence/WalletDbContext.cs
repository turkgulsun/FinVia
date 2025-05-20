using Microsoft.EntityFrameworkCore;
using WalletService.Domain.Entities;
using WalletService.Domain.ValueObjects;

namespace WalletService.Infrastructure.Persistence;

public class WalletDbContext : DbContext
{
    public WalletDbContext(DbContextOptions<WalletDbContext> options) : base(options) { }

    public DbSet<Wallet> Wallets => Set<Wallet>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Wallet>(wallet =>
        {
            wallet.HasKey(w => w.Id);

            wallet.Property(w => w.Id).HasConversion(
                id => id.Value,
                value => WalletId.From(value)
            );
            
            wallet.OwnsOne(w => w.Balance, balance =>
            {
                balance.Property(b => b.Amount).HasColumnName("Amount");
                balance.Property(b => b.Currency).HasColumnName("Currency");
            });

            wallet.Property(w => w.UserId);
            wallet.Property(w => w.Currency);
            wallet.Property(w => w.CreatedAt);
            wallet.Property(w => w.IsActive);
        });
    }
}
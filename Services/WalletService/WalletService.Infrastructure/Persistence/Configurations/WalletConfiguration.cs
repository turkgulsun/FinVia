using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WalletService.Domain.Entities;
using WalletService.Domain.ValueObjects;

namespace WalletService.Infrastructure.Persistence.Configurations;

public class WalletConfiguration : IEntityTypeConfiguration<Wallet>
{
    public void Configure(EntityTypeBuilder<Wallet> wallet)
    {
        wallet.HasKey(w => w.Id);

        wallet.Property(w => w.Id)
            .HasConversion(id => id, value => WalletId.From(value));

        wallet.OwnsOne(w => w.Balance, balance =>
        {
            balance.Property(b => b.Amount).HasColumnName("Amount");
            balance.Property(b => b.Currency).HasColumnName("Currency");
        });

        wallet.Property(w => w.UserId);
        wallet.Property(w => w.Currency);
        wallet.Property(w => w.CreatedAt);
        wallet.Property(w => w.IsActive);
    }
}
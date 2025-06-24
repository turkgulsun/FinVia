using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WalletService.Domain.Entities;
using WalletService.Domain.Enums;

namespace WalletService.Infrastructure.Persistence.Configurations;

public class WalletTransactionConfiguration : IEntityTypeConfiguration<WalletTransaction>
{
    public void Configure(EntityTypeBuilder<WalletTransaction> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.WalletId);
        builder.Property(x => x.Amount);
        builder.Property(x => x.Currency)
            .HasConversion(x => x.ToString(), x => Enum.Parse<Currency>(x));
        builder.Property(x => x.Description).HasMaxLength(250);
        builder.Property(x => x.CreatedAt);
    }
}
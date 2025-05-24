using KycService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KycService.Infrastructure.Persistence.Configurations;

public class KycConfiguration : IEntityTypeConfiguration<Kyc>
{
    public void Configure(EntityTypeBuilder<Kyc> builder)
    {
        builder.HasKey(k => k.Id);
        builder.Property(k => k.FirstName).HasMaxLength(50).IsRequired();
        builder.Property(k => k.LastName).HasMaxLength(50).IsRequired();
        builder.Property(k => k.NationalId).HasMaxLength(20).IsRequired();
        builder.Property(k => k.CountryCode).HasMaxLength(2).IsRequired();
        builder.Property(k => k.Status).IsRequired();
    }
}
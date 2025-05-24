using KycService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KycService.Infrastructure.Persistence.Configurations;

public class KycAuditLogConfiguration : IEntityTypeConfiguration<KycAuditLog>
{
    public void Configure(EntityTypeBuilder<KycAuditLog> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.Status).IsRequired();
        builder.Property(x => x.Message).HasMaxLength(250).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
    }
}

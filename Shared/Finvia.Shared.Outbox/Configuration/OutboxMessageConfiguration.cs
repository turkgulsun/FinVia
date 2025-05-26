using Finvia.Shared.Outbox.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Finvia.Shared.Outbox.Configuration;

public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> entity)
    {
        entity.ToTable("OutboxMessages");
        
        entity.HasKey(e => e.Id);
        entity.Property(e => e.EventType).IsRequired();
        entity.Property(e => e.Payload).IsRequired();
        entity.Property(e => e.Status).IsRequired();
        entity.Property(e => e.CorrelationId).IsRequired();
        entity.Property(e => e.RetryCount).HasDefaultValue(0);
        entity.Property(e => e.CreatedAt).IsRequired();
        entity.Property(e => e.ProcessedAt);
        entity.Property(e => e.FailedAt);
    }
}
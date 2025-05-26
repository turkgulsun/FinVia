using Finvia.Shared.Outbox.Abstractions;
using Finvia.Shared.Outbox.Models;
using Finvia.Shared.Outbox.Configuration;
using KycService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KycService.Infrastructure.Persistence;

public class KycDbContext : DbContext, IOutboxDbContext
{
    public DbSet<Kyc> Kycs => Set<Kyc>();
    public DbSet<KycAuditLog> AuditLogs => Set<KycAuditLog>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    public KycDbContext(DbContextOptions<KycDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new OutboxMessageConfiguration());
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(KycDbContext).Assembly);
    }
}
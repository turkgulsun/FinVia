using KycService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KycService.Infrastructure.Persistence;

public class KycDbContext : DbContext
{
    public DbSet<Kyc> Kycs => Set<Kyc>();
    public DbSet<KycAuditLog> AuditLogs => Set<KycAuditLog>();


    public KycDbContext(DbContextOptions<KycDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(KycDbContext).Assembly);
    }
}
using Finvia.Shared.Outbox.Abstractions;
using Finvia.Shared.Outbox.Configuration;
using Finvia.Shared.Outbox.Models;
using Microsoft.EntityFrameworkCore;

namespace CampaignService.OutboxPublisherWorker.Persistence;

public class CampaignDbContext : DbContext, IOutboxDbContext
{
    public CampaignDbContext(DbContextOptions<CampaignDbContext> options) : base(options)
    {
    }

    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => base.SaveChangesAsync(cancellationToken);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new OutboxMessageConfiguration());
    }
}
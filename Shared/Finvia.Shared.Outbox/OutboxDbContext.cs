using Microsoft.EntityFrameworkCore;
using Finvia.Shared.Outbox.Models;
using Finvia.Shared.Outbox.Abstractions;
using Finvia.Shared.Outbox.Configuration;

namespace Finvia.Shared.Outbox;

public class OutboxDbContext : DbContext, IOutboxDbContext
{
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    public OutboxDbContext(DbContextOptions<OutboxDbContext> options)
        : base(options) { }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => base.SaveChangesAsync(cancellationToken);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new OutboxMessageConfiguration());
    }
}
using Microsoft.EntityFrameworkCore;

using Finvia.Shared.Outbox.Models;

namespace Finvia.Shared.Outbox.Abstractions;

public interface IOutboxDbContext
{
    DbSet<OutboxMessage> OutboxMessages { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
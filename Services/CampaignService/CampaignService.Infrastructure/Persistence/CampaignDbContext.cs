using CampaignService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CampaignService.Infrastructure.Persistence;

public class CampaignDbContext : DbContext
{
    public DbSet<CampaignReward> Rewards => Set<CampaignReward>();

    public CampaignDbContext(DbContextOptions<CampaignDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CampaignDbContext).Assembly);
    }
}
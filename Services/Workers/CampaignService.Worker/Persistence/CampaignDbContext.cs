using CampaignService.Worker.Models;
using Microsoft.EntityFrameworkCore;

namespace CampaignService.Worker.Persistence;

public class CampaignDbContext : DbContext
{
    public CampaignDbContext(DbContextOptions<CampaignDbContext> options)
        : base(options) { }

    public DbSet<CampaignReward> CampaignRewards => Set<CampaignReward>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CampaignReward>(entity =>
        {
            entity.ToTable("CampaignRewards");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UserId).IsRequired();
            entity.Property(e => e.Amount).IsRequired();
            entity.Property(e => e.Currency).IsRequired();
            entity.Property(e => e.CorrelationId).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
        });
    }
}
using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;
using UserService.Domain.ValueObjects;

namespace UserService.Infrastructure.Persistence;

public class UserDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();

    public UserDbContext(DbContextOptions<UserDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            
            entity.Property(u => u.Id).HasConversion(
                id => id.Value,
                value => UserId.From(value)
            );

            entity.Property(u => u.FullName)
                .IsRequired();

            entity.Property(u => u.Email)
                .HasColumnName("Email")
                .IsRequired();

            entity.Property(u => u.Status)
                .HasConversion<string>();
        });
    }
}
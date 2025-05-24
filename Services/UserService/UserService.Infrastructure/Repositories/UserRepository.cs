using Microsoft.EntityFrameworkCore;
using UserService.Domain.Abstractions;
using UserService.Domain.Entities;
using UserService.Infrastructure.Persistence;

namespace UserService.Infrastructure.Repositories;

public class UserRepository(UserDbContext db) : IUserRepository
{
    public async Task AddAsync(User user)
    {
        db.Users.Add(user);
        await db.SaveChangesAsync();
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
    }
}
using UserService.Domain.Entities;

namespace UserService.Domain.Abstractions;

public interface IUserRepository
{
    Task AddAsync(User user);
    Task<User?> GetByEmailAsync(string email);
}
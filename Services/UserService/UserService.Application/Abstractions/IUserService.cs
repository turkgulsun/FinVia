using UserService.Domain.Entities;

namespace UserService.Application.Abstractions;

public interface IUserService
{
    Task AddAsync(User user);
    Task<User?> GetByEmailAsync(string email);
}
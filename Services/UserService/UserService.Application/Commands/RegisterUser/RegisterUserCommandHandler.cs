using Finvia.Shared.Common;
using Finvia.Shared.Utilities;
using MediatR;
using UserService.Application.Abstractions;
using UserService.Domain.Entities;
using UserService.Domain.Messages;

namespace UserService.Application.Commands.RegisterUser;

public class RegisterUserCommandHandler(IUserService userService) : IRequestHandler<RegisterUserCommand, Result<Guid>>
{

    public async Task<Result<Guid>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        // Email already exists check
        var exists = await userService.GetByEmailAsync(command.Email);
        if (exists != null)
            return Result<Guid>.Failure(ValidationMessages.EmailAlreadyExists);

        var passwordHash = PasswordHasher.Hash(command.Password);
        var user = new User(command.FullName, command.Email, passwordHash, command.PhoneNumber);

        await userService.AddAsync(user);

        return Result<Guid>.Success(user.Id, Messages.UserRegistered);
    }
}

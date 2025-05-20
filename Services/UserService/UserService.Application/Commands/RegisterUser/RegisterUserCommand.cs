using Finvia.Shared.Common;
using MediatR;

namespace UserService.Application.Commands.RegisterUser;

public record RegisterUserCommand(
    string FullName,
    string Email,
    string Password,
    string PhoneNumber
) : IRequest<Result<Guid>>;
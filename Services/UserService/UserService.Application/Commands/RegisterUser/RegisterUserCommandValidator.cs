
using FluentValidation;
using Microsoft.AspNetCore.Components.Forms;
using UserService.Domain.Messages;

namespace UserService.Application.Commands.RegisterUser;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage(ValidationMessages.FullNameRequired);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(ValidationMessages.EmailRequired)
            .EmailAddress().WithMessage(ValidationMessages.EmailInvalid);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(ValidationMessages.PasswordRequired)
            .MinimumLength(6).WithMessage(ValidationMessages.PasswordTooShort);

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage(ValidationMessages.PhoneRequired)
            .Matches(@"^[0-9]+$").WithMessage(ValidationMessages.PhoneInvalid);
    }
}

using FluentValidation;
using Microsoft.AspNetCore.Components.Forms;
using UserService.Application.DTOs;
using UserService.Domain.Messages;

namespace UserService.API.Validators;

public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
{
    public RegisterUserRequestValidator()
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

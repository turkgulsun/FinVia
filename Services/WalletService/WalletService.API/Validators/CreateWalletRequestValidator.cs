using FluentValidation;
using WalletService.Application.DTOs;
using WalletService.Domain.Messages;

namespace WalletService.API.Validators;

public class CreateWalletRequestValidator : AbstractValidator<CreateWalletRequest>
{
    public CreateWalletRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage(ValidationMessages.UserIdRequired);

        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage(ValidationMessages.CurrencyRequired)
            .Must(c => new[] { "TRY", "USD", "EUR" }.Contains(c))
            .WithMessage(ValidationMessages.CurrencyInvalid);
    }
}
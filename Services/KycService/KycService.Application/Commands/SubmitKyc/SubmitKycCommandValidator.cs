using FluentValidation;
using KycService.Application.Commands.SubmitKyc;

namespace KycService.Application.Commands.Kyc;

public class SubmitKycCommandValidator : AbstractValidator<SubmitKycCommand>
{
    public SubmitKycCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.NationalId).NotEmpty().Length(11); // TR için örnek
        RuleFor(x => x.DateOfBirth).LessThan(DateTime.UtcNow).WithMessage("Date of birth must be in the past.");
        RuleFor(x => x.CountryCode).NotEmpty().Length(2);
    }
}

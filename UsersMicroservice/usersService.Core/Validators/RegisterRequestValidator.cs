using FluentValidation;
using usersService.Core.Dtos;

namespace usersService.Core.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email address.");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long");
        RuleFor(x => x.PersonName).NotEmpty().WithMessage("Person name is required.");
        RuleFor(x => x.Gender)
            .NotEmpty().WithMessage("Gender is required.")
            .IsInEnum().WithMessage("Gender must be either Male or Female or Other");
    }
}
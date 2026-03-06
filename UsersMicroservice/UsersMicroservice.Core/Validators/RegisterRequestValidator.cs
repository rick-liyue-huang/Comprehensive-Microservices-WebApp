using FluentValidation;
using UsersMicroservice.Core.Dtos;

namespace UsersMicroservice.Core.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.");
        RuleFor(x => x.PersonName)
            .NotEmpty().WithMessage("Person name is required.")
            .Length(3, 50).WithMessage("Person name must be between 3 and 50 characters.");
        RuleFor(x => x.Gender)
            .NotEmpty().WithMessage("Gender is required.");
    }
}
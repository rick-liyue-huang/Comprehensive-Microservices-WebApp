
using FluentValidation;
using UsersMicroservice.Core.Dtos;

namespace UsersMicroservice.Core.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
  public RegisterRequestValidator()
  {
    RuleFor(x => x.Email)
      .NotEmpty()
      .WithMessage("Email is required")
      .EmailAddress()
      .WithMessage("Email is not valid");
    RuleFor(x => x.Password)
      .NotEmpty()
      .WithMessage("Password is required")
      .MinimumLength(6)
      .WithMessage("Password must be at least 6 characters long");
    RuleFor(x => x.PersonName).NotEmpty().WithMessage("Person name is required");
    RuleFor(x => x.Gender).IsInEnum().WithMessage("Gender is required");
  }
}

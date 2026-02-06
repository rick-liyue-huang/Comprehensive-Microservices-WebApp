using FluentValidation;
using usersService.Core.Dtos;

namespace usersService.Core.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        // Email and Password are required
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email address.");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");
    }
}
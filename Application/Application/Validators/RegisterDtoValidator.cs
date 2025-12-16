using Application.DTOs.Auth;
using FluentValidation;

namespace Application.Validators
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .MaximumLength(100).WithMessage("First name must not exceed 100 characters");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .MaximumLength(100).WithMessage("Last name must not exceed 100 characters");

            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required")
                .MaximumLength(50).WithMessage("Username must not exceed 50 characters");

            RuleFor(x => x.Age)
                .GreaterThan(0).WithMessage("Age must be greater than 0")
                .LessThanOrEqualTo(150).WithMessage("Age must be less than or equal to 150");

            RuleFor(x => x.MonthlyIncome)
                .GreaterThanOrEqualTo(0).WithMessage("Monthly income must be greater than or equal to 0");
        }
    }
}



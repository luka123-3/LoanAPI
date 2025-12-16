using Application.DTOs.Loan;
using FluentValidation;

namespace Application.Validators
{
    public class CreateLoanDtoValidator : AbstractValidator<CreateLoanDto>
    {
        public CreateLoanDtoValidator()
        {
            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("Invalid loan type");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than 0");

            RuleFor(x => x.Currency)
                .NotEmpty().WithMessage("Currency is required")
                .Length(3).WithMessage("Currency must be 3 characters (e.g., USD, EUR, GEL)");

            RuleFor(x => x.DurationMonths)
                .GreaterThan(0).WithMessage("Duration must be greater than 0")
                .LessThanOrEqualTo(360).WithMessage("Duration must not exceed 360 months (30 years)");
        }
    }
}



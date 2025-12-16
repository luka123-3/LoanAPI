using Application.DTOs.Loan;
using FluentValidation;

namespace Application.Validators
{
    public class UpdateLoanDtoValidator : AbstractValidator<UpdateLoanDto>
    {
        public UpdateLoanDtoValidator()
        {
            RuleFor(x => x.Amount)
                .GreaterThan(0).When(x => x.Amount.HasValue)
                .WithMessage("Amount must be greater than 0");

            RuleFor(x => x.Currency)
                .Length(3).When(x => !string.IsNullOrEmpty(x.Currency))
                .WithMessage("Currency must be 3 characters (e.g., USD, EUR, GEL)");

            RuleFor(x => x.DurationMonths)
                .GreaterThan(0).When(x => x.DurationMonths.HasValue)
                .WithMessage("Duration must be greater than 0")
                .LessThanOrEqualTo(360).When(x => x.DurationMonths.HasValue)
                .WithMessage("Duration must not exceed 360 months (30 years)");
        }
    }
}



using Application.DTOs.Loan;
using FluentValidation;

namespace Application.Validators
{
    public class UpdateLoanStatusDtoValidator : AbstractValidator<UpdateLoanStatusDto>
    {
        public UpdateLoanStatusDtoValidator()
        {
            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid loan status");
        }
    }
}



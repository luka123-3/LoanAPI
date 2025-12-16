using Application.DTOs.User;
using FluentValidation;

namespace Application.Validators
{
    public class BlockUserDtoValidator : AbstractValidator<BlockUserDto>
    {
        public BlockUserDtoValidator()
        {
            RuleFor(x => x.BlockedUntil)
                .GreaterThan(DateTime.UtcNow).When(x => x.BlockedUntil.HasValue)
                .WithMessage("Blocked until date must be in the future");
        }
    }
}



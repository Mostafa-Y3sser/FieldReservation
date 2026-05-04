using FluentValidation;

namespace FieldReservation.Application.Admin.Commands.UnblockUser
{
    public class UnblockUserCommandValidator : AbstractValidator<UnblockUserCommand>
    {
        public UnblockUserCommandValidator()
        {
            RuleFor(v => v.UserId)
                .NotEmpty().WithMessage("User ID is required.");
        }
    }
}

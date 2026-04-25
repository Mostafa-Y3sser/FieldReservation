using FluentValidation;

namespace FieldReservation.Application.Admin.Commands.BlockUser
{
    public class BlockUserCommandValidator : AbstractValidator<BlockUserCommand>
    {
        public BlockUserCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.")
                .Must(id => Guid.TryParse(id, out _)).WithMessage("UserId must be a valid GUID.");
        }
    }
}
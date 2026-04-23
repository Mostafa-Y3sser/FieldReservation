using FluentValidation;

namespace FieldReservation.Application.Admin.Commands.OverrideReservation
{
    public class OverrideReservationCommandValidator : AbstractValidator<OverrideReservationCommand>
    {
        public OverrideReservationCommandValidator()
        {
            RuleFor(x => x.ReservationId)
                .NotEmpty().WithMessage("Reservation ID is required.");

            RuleFor(x => x.NewStatus)
                .NotEmpty().WithMessage("New reservation status is required.")
                .IsInEnum().WithMessage("Invalid reservation status.");
        }
    }
}
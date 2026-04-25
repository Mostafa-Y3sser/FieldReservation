using FluentValidation;

namespace FieldReservation.Application.Admin.Commands.CancelReservation
{
    public class CancelReservationCommandValidator : AbstractValidator<CancelReservationCommand>
    {
        public CancelReservationCommandValidator()
        {
            RuleFor(x => x.ReservationId)
                .NotEmpty().WithMessage("Reservation ID is required.");

            RuleFor(x => x.NewStatus)
                .NotEmpty().WithMessage("New reservation status is required.")
                .IsInEnum().WithMessage("Invalid reservation status.");
        }
    }
}
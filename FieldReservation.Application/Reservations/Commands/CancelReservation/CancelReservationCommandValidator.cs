
using FluentValidation;

namespace FieldReservation.Application.Reservations.Commands.CancelReservation
{
    public class CancelReservationCommandValidator : AbstractValidator<CancelReservationCommand>
    {
        public CancelReservationCommandValidator()
        {
            RuleFor(x => x.Id)
                    .NotEmpty().WithMessage("Reservation ID is required.");
        }
    }
}

using FluentValidation;

namespace FieldReservation.Application.Reservations.Queries.GetReservation
{
    public class GetReservationQueryValidator : AbstractValidator<GetReservationQuery>
    {
        public GetReservationQueryValidator()
        {
            RuleFor(q => q.ReservationId)
                .NotEmpty().WithMessage("Reservation ID is required.")
                .Must(id => Guid.TryParse(id.ToString(), out _)).WithMessage("Invalid Reservation ID format.");
        }
    }
}
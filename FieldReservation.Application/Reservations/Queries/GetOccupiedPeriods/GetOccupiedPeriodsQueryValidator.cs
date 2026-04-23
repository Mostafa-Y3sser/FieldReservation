using FluentValidation;

namespace FieldReservation.Application.Reservations.Queries.GetOccupiedPeriods
{
    public class GetOccupiedPeriodsQueryValidator : AbstractValidator<GetOccupiedPeriodsQuery>
    {
        public GetOccupiedPeriodsQueryValidator()
        {
            RuleFor(q => q.Date)
                .NotEmpty().WithMessage("Date is required.");
        }
    }
}
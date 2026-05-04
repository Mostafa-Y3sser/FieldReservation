using FluentValidation;

namespace FieldReservation.Application.Admin.Queries.GetUserReservations
{
    public class GetUserReservationsQueryValidator : AbstractValidator<GetUserReservationsQuery>
    {
        public GetUserReservationsQueryValidator()
        {
            RuleFor(v => v.UserId)
                .NotEmpty().WithMessage("User ID is required.");
        }
    }
}

using FluentValidation;

namespace FieldReservation.Application.Reservations.Commands.CreateReservation
{
    public class CreateReservationCommandValidator
        : AbstractValidator<CreateReservationCommand>
    {
        public CreateReservationCommandValidator()
        {
            RuleFor(x => x.FieldId).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.StartTime).LessThan(x => x.EndTime);
            RuleFor(x => x.EndTime).GreaterThan(DateTime.UtcNow);
        }
    }
}
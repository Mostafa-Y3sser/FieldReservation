using FluentValidation;

namespace FieldReservation.Application.Admin.Commands.CreateMaintenance
{
    public class CreateMaintenanceCommandValidator : AbstractValidator<CreateMaintenanceCommand>
    {
        public CreateMaintenanceCommandValidator()
        {
            RuleFor(x => x.StartTime)
                .NotEmpty().WithMessage("Start time is required.")
                .Must(startTime => startTime > DateTime.Now).WithMessage("Start time must be in the future.")
                .Must(startTime => startTime < DateTime.Now.AddDays(7)).WithMessage("Reservations can only be made up to 7 days in advance.");

            RuleFor(x => x.EndTime)
                .NotEmpty().WithMessage("End time is required.")
                .GreaterThan(x => x.StartTime).WithMessage("End time must be after start time.")
                .Must(endTime => endTime < DateTime.Now.AddDays(8)).WithMessage("End time exceeds the 7-day booking window.");

            RuleFor(x => x.Note)
                .NotEmpty().WithMessage("Note is required.")
                .MaximumLength(150).WithMessage("Note cannot exceed 150 characters.");
        }
    }
}
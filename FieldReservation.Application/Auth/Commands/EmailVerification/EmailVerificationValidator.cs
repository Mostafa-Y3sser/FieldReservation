using FluentValidation;

namespace FieldReservation.Application.Auth.Commands.EmailVerification
{
    public class EmailVerificationValidator : AbstractValidator<EmailVerificationCommand>
    {
        public EmailVerificationValidator()
        {
            RuleFor(x => x.Email)
              .Cascade(CascadeMode.Stop)
              .NotEmpty().WithMessage("Email is required.")
              .EmailAddress().WithMessage("A valid email required.")
              .Must(x => x.EndsWith("@gmail.com")).WithMessage("Only Gmail addresses are allowed.");

            RuleFor(x => x.Token)
               .NotEmpty().WithMessage("Token is required.");
        }
    }
}
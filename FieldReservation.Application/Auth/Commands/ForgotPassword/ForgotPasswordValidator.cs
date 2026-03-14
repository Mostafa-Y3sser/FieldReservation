using FluentValidation;

namespace FieldReservation.Application.Auth.Commands.ForgotPassword
{
    public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordCommand>
    {
        public ForgotPasswordValidator()
        {
            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .Must(x => x.EndsWith("@gmail.com")).WithMessage("Only Gmail addresses are allowed.");
        }
    }
}
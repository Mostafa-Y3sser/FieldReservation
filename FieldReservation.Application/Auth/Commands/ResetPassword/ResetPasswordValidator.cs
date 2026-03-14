using FluentValidation;

namespace FieldReservation.Application.Auth.Commands.ResetPassword
{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email is required.")
                .Must(x => x.EndsWith("@gmail.com")).WithMessage("Only Gmail addresses are allowed.");

            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("Token is required.");

            RuleFor(x => x.NewPassword)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain at least one digit.")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one non-alphanumeric character.");

            RuleFor(x => x.ConfirmNewPassword)
                .Equal(x => x.NewPassword).WithMessage("Confirm password must match new password.");
        }
    }
}
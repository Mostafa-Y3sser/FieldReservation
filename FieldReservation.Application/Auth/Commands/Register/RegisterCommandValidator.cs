using PhoneNumbers;
using FluentValidation;

namespace FieldReservation.Application.Auth.Commands.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.FirstName)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty().WithMessage("First name is required.")
                    .MinimumLength(3).WithMessage("First name must be at least 3 characters.")
                    .MaximumLength(15).WithMessage("First name must not exceed 30 characters.");

            RuleFor(x => x.LastName)
                   .Cascade(CascadeMode.Stop)
                   .NotEmpty().WithMessage("Last name is required.")
                   .MinimumLength(3).WithMessage("Last name must be at least 3 characters.")
                   .MaximumLength(15).WithMessage("Last name must not exceed 30 characters.");

            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .Must(x => x.EndsWith("@gmail.com")).WithMessage("Only Gmail addresses are allowed.");

            RuleFor(x => x)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Phone number is required.")
                .Must(x => beValidPhoneNumber(x.PhoneNumber)).WithMessage("Invalid phone number format.");

            RuleFor(x => x.Password)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain at least one digit.")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one non-alphanumeric character.");
        }

        private bool beValidPhoneNumber(string phoneNumber)
        {
            PhoneNumberUtil phoneUtil = PhoneNumberUtil.GetInstance();

            try
            {
                var parsedNumber = phoneUtil.Parse(phoneNumber, "EG");
                return phoneUtil.IsValidNumberForRegion(parsedNumber, "EG");
            }
            catch
            {
                return false;
            }
        }
    }
}
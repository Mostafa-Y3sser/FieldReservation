using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace FieldReservation.Application.Auth.Commands.SendEmailVerificationToken
{
    public class SendEmailVerificationTokenValidator : AbstractValidator<SendEmailVerificationTokenCommand>
    {
        public SendEmailVerificationTokenValidator()
        {
            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .Must(x => x.EndsWith("@gmail.com")).WithMessage("Only Gmail addresses are allowed.");
        }
    }
}

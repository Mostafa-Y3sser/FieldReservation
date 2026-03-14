using FluentValidation;

namespace FieldReservation.Application.Auth.Commands.GoogleSignIn
{
    public class GoogleSignInValidator:AbstractValidator<GoogleSignInCommand>
    {
        public GoogleSignInValidator()
        {
            RuleFor(x => x.IdToken)
                .NotEmpty().WithMessage("Id token is required.");
        }
    }
}
using FluentValidation;

namespace FieldReservation.Application.Auth.Commands.GoogleSignUp
{
    public class GoogleSignUpValidator : AbstractValidator<GoogleSignUpCommand>
    {
        public GoogleSignUpValidator()
        {
            RuleFor(x => x.IdToken)
                .NotEmpty().WithMessage("Id token is required.");
        }
    }
}
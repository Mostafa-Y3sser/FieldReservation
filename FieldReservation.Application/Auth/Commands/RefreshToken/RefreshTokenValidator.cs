using FluentValidation;

namespace FieldReservation.Application.Auth.Commands.RefreshToken
{
    public class RefreshTokenValidation : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenValidation()
        {
            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("Refresh token is required.");
        }
    }
}
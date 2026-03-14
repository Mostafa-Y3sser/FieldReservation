using FluentValidation;

namespace FieldReservation.Application.Auth.Commands.RevokeRefreshToken
{
    public class RevokeRefreshTokenValidator : AbstractValidator<RevokeRefreshTokenCommand>
    {
        public RevokeRefreshTokenValidator()
        {
            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("Refresh token is required.");
        }
    }
}
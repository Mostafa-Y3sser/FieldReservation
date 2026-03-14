using MediatR;
using FieldReservation.Application.Common.Results;
using FieldReservation.Application.Common.Interfaces;

namespace FieldReservation.Application.Auth.Commands.RevokeRefreshToken
{
    public sealed class RevokeRefreshTokenCommandHandler(IAuthService authService)
        : IRequestHandler<RevokeRefreshTokenCommand, Result>
    {
        public Task<Result> Handle(RevokeRefreshTokenCommand request, CancellationToken cancellationToken)
            => authService.RevokeRefreshTokenAsync(request.Token);
    }
}
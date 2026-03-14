using MediatR;
using FieldReservation.Application.Auth.Dtos;
using FieldReservation.Application.Common.Results;
using FieldReservation.Application.Common.Interfaces;

namespace FieldReservation.Application.Auth.Commands.RefreshToken
{
    public sealed class RefreshTokenCommandHandler(IAuthService authService)
        : IRequestHandler<RefreshTokenCommand, Result<AuthResponseDto>>
    {
        public Task<Result<AuthResponseDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
            => authService.RefreshTokenAsync(request.Token);
    }
}
using MediatR;
using FieldReservation.Application.Auth.Dtos;
using FieldReservation.Application.Common.Results;
using FieldReservation.Application.Common.Interfaces;

namespace FieldReservation.Application.Auth.Commands.GoogleSignIn
{
    public sealed class GoogleSignInCommandHandler(IAuthService authService)
        : IRequestHandler<GoogleSignInCommand, Result<AuthResponseDto>>
    {
        public Task<Result<AuthResponseDto>> Handle(GoogleSignInCommand request, CancellationToken cancellationToken)
            => authService.GoogleSignInAsync(request.IdToken);
    }
}
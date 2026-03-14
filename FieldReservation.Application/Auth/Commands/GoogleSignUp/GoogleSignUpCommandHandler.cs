using MediatR;
using FieldReservation.Application.Auth.Dtos;
using FieldReservation.Application.Common.Results;
using FieldReservation.Application.Common.Interfaces;

namespace FieldReservation.Application.Auth.Commands.GoogleSignUp
{
    public sealed class GoogleSignUpCommandHandler(IAuthService authService)
        : IRequestHandler<GoogleSignUpCommand, Result<AuthResponseDto>>
    {
        public Task<Result<AuthResponseDto>> Handle(GoogleSignUpCommand request, CancellationToken cancellationToken)
            => authService.GoogleSignUpAsync(request.IdToken);
    }
}
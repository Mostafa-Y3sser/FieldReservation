using MediatR;
using FieldReservation.Application.Auth.Dtos;
using FieldReservation.Application.Common.Results;
using FieldReservation.Application.Common.Interfaces;

namespace FieldReservation.Application.Auth.Commands.Login
{
    public sealed class LoginCommandHandler(IAuthService authService)
        : IRequestHandler<LoginCommand, Result<AuthResponseDto>>
    {
        public Task<Result<AuthResponseDto>> Handle(
            LoginCommand command,
            CancellationToken cancellationToken)
            => authService.LoginAsync(command.Email, command.Password, cancellationToken);
    }
}
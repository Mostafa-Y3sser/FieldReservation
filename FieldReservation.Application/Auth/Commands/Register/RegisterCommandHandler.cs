using MediatR;
using FieldReservation.Application.Auth.Dtos;
using FieldReservation.Application.Common.Results;
using FieldReservation.Application.Common.Interfaces;

namespace FieldReservation.Application.Auth.Commands.Register
{
    public sealed class RegisterCommandHandler(IAuthService authService)
        : IRequestHandler<RegisterCommand, Result<AuthResponseDto>>
    {
        public Task<Result<AuthResponseDto>> Handle(
            RegisterCommand request,
            CancellationToken cancellationToken)
            => authService.RegisterAsync(request.FirstName, request.LastName, request.Email, request.PhoneNumber,
                request.Password, request.ConfirmPassword, cancellationToken);
    }
}
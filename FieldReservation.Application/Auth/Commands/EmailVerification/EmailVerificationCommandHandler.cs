using MediatR;
using FieldReservation.Application.Auth.Dtos;
using FieldReservation.Application.Common.Results;
using FieldReservation.Application.Common.Interfaces;

namespace FieldReservation.Application.Auth.Commands.EmailVerification
{
    public sealed class EmailVerificationCommandHandler(IAuthService authService)
        : IRequestHandler<EmailVerificationCommand, Result<AuthResponseDto>>
    {
        public Task<Result<AuthResponseDto>> Handle(EmailVerificationCommand request, CancellationToken cancellationToken)
            => authService.EmailVerificationAsync(request.Email, request.Token);
    }
}
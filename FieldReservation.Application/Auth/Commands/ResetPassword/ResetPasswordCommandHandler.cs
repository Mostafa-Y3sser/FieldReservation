using MediatR;
using FieldReservation.Application.Common.Results;
using FieldReservation.Application.Common.Interfaces;

namespace FieldReservation.Application.Auth.Commands.ResetPassword
{
    public sealed class ResetPasswordCommandHandler(IAuthService authService)
        : IRequestHandler<ResetPasswordCommand, Result>
    {
        public Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
            => authService.ResetPasswordAsync(request.Email, request.Token, request.NewPassword, request.ConfirmNewPassword);
    }
}
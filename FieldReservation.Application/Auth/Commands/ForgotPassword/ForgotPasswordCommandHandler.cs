using MediatR;
using FieldReservation.Application.Common.Results;
using FieldReservation.Application.Common.Interfaces;

namespace FieldReservation.Application.Auth.Commands.ForgotPassword
{
    public sealed class ForgotPasswordCommandHandler(IAuthService authService)
        : IRequestHandler<ForgotPasswordCommand, Result>
    {
        public Task<Result> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
            => authService.ForgotPasswordAsync(request.Email);
    }
}
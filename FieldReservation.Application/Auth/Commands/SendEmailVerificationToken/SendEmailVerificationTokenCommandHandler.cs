using MediatR;
using FieldReservation.Application.Common.Results;
using FieldReservation.Application.Common.Interfaces;

namespace FieldReservation.Application.Auth.Commands.SendEmailVerificationToken
{
    public sealed class SendEmailVerificationTokenCommandHandler(IAuthService authService)
        : IRequestHandler<SendEmailVerificationTokenCommand, Result>
    {
        public Task<Result> Handle(SendEmailVerificationTokenCommand request, CancellationToken cancellationToken)
            => authService.SendEmailVerificationTokenAsync(request.Email);
    }
}
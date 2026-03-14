using MediatR;
using FieldReservation.Application.Auth.Dtos;
using FieldReservation.Application.Common.Results;

namespace FieldReservation.Application.Auth.Commands.EmailVerification
{
    public record EmailVerificationCommand(string Email, string Token) : IRequest<Result<AuthResponseDto>>;
}
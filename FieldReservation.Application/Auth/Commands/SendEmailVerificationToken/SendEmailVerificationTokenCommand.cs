using MediatR;
using FieldReservation.Application.Common.Results;

namespace FieldReservation.Application.Auth.Commands.SendEmailVerificationToken
{
    public record SendEmailVerificationTokenCommand(string Email) : IRequest<Result>;
}
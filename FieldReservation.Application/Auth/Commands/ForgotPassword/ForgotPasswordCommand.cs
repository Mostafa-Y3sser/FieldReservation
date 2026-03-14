using MediatR;
using FieldReservation.Application.Common.Results;

namespace FieldReservation.Application.Auth.Commands.ForgotPassword
{
    public record ForgotPasswordCommand(string Email) : IRequest<Result>;
}
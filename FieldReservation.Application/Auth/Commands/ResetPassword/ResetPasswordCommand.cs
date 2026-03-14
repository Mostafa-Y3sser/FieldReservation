using MediatR;
using FieldReservation.Application.Common.Results;

namespace FieldReservation.Application.Auth.Commands.ResetPassword
{
    public record ResetPasswordCommand(string Email, string Token, string NewPassword, string ConfirmNewPassword) : IRequest<Result>;
}
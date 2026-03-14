using MediatR;
using FieldReservation.Application.Auth.Dtos;
using FieldReservation.Application.Common.Results;

namespace FieldReservation.Application.Auth.Commands.Login
{
    public record LoginCommand(string Email, string Password) : IRequest<Result<AuthResponseDto>>;
}
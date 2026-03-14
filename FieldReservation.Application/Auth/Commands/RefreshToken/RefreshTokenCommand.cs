using MediatR;
using FieldReservation.Application.Auth.Dtos;
using FieldReservation.Application.Common.Results;

namespace FieldReservation.Application.Auth.Commands.RefreshToken
{
    public record RefreshTokenCommand(string Token) : IRequest<Result<AuthResponseDto>>;
}
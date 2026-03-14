using MediatR;
using FieldReservation.Application.Common.Results;

namespace FieldReservation.Application.Auth.Commands.RevokeRefreshToken
{
    public record RevokeRefreshTokenCommand(string Token) : IRequest<Result>;
}
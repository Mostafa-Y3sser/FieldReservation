using MediatR;
using FieldReservation.Application.Auth.Dtos;
using FieldReservation.Application.Common.Results;

namespace FieldReservation.Application.Auth.Commands.GoogleSignIn
{
    public record GoogleSignInCommand(string IdToken) : IRequest<Result<AuthResponseDto>>;
}
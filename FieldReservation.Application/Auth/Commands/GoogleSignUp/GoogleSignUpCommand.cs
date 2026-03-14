using MediatR;
using FieldReservation.Application.Auth.Dtos;
using FieldReservation.Application.Common.Results;

namespace FieldReservation.Application.Auth.Commands.GoogleSignUp
{
    public record GoogleSignUpCommand(string IdToken) : IRequest<Result<AuthResponseDto>>;
}
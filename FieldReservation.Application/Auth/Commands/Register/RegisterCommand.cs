using MediatR;
using FieldReservation.Application.Auth.Dtos;
using FieldReservation.Application.Common.Results;

namespace FieldReservation.Application.Auth.Commands.Register
{
    public record RegisterCommand(string FirstName, string LastName, string Email, string PhoneNumber,
        string Password, string ConfirmPassword) : IRequest<Result<AuthResponseDto>>;
}
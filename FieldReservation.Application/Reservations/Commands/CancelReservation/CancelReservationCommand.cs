using MediatR;
using FieldReservation.Application.Common.Results;

namespace FieldReservation.Application.Reservations.Commands.CancelReservation
{
    public record CancelReservationCommand(
           Guid Id) : IRequest<Result>;
}
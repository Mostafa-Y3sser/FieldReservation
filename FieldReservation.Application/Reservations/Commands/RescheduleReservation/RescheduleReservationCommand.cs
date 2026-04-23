using MediatR;
using FieldReservation.Application.Common.Results;

namespace FieldReservation.Application.Reservations.Commands.RescheduleReservation
{
    public record RescheduleReservationCommand(
    Guid Id,
    DateTime StartTime,
    DateTime EndTime) : IRequest<Result>;
}
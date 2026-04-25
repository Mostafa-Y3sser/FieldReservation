using MediatR;
using FieldReservation.Domain.Enums;
using FieldReservation.Application.Common.Results;

namespace FieldReservation.Application.Admin.Commands.CancelReservation
{
    public record CancelReservationCommand(
     Guid ReservationId,
     ReservationStatus NewStatus) : IRequest<Result>;
}
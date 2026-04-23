using MediatR;
using FieldReservation.Domain.Enums;
using FieldReservation.Application.Common.Results;

namespace FieldReservation.Application.Admin.Commands.OverrideReservation
{
    public record OverrideReservationCommand(
     Guid ReservationId,
     ReservationStatus NewStatus) : IRequest<Result>;
}
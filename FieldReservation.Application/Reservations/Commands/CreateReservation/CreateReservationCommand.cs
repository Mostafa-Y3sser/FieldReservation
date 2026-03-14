using MediatR;
using FieldReservation.Application.Common.Results;

namespace FieldReservation.Application.Reservations.Commands.CreateReservation
{
    public record CreateReservationCommand(
        Guid FieldId,
        Guid UserId,
        DateTime StartTime,
        DateTime EndTime) : IRequest<Result<Guid>>;
}
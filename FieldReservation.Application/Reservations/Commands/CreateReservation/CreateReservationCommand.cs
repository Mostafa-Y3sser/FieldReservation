using MediatR;
using FieldReservation.Application.Common.Results;

namespace FieldReservation.Application.Reservations.Commands.CreateReservation
{
    public record CreateReservationCommand(
        DateTime StartTime,
        DateTime EndTime) : IRequest<Result<CreateReservationResponse>>;
}
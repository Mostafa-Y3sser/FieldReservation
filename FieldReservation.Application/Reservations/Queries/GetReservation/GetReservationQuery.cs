using MediatR;
using FieldReservation.Application.Common.Results;

namespace FieldReservation.Application.Reservations.Queries.GetReservation
{

    public record GetReservationQuery(
        Guid ReservationId) : IRequest<Result<ReservationResponse>>;
}
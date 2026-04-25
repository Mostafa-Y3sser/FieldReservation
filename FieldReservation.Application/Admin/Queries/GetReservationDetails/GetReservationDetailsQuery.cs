using FieldReservation.Application.Common.Results;
using MediatR;

namespace FieldReservation.Application.Admin.Queries.GetReservationDetails;

public record GetReservationDetailsQuery(Guid ReservationId)
    : IRequest<Result<AdminReservationDetailsResponse>>;
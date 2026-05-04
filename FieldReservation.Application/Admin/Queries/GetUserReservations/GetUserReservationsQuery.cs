using FieldReservation.Application.Admin.Queries.GetAllReservations;
using FieldReservation.Application.Common.Results;
using MediatR;

namespace FieldReservation.Application.Admin.Queries.GetUserReservations;

public record GetUserReservationsQuery(string UserId) : IRequest<Result<List<ReservationDto>>>;

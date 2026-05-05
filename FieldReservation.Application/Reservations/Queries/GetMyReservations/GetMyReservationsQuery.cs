using FieldReservation.Application.Common.Results;
using MediatR;

namespace FieldReservation.Application.Reservations.Queries.GetMyReservations;

public record GetMyReservationsQuery : IRequest<Result<List<MyReservationResponse>>>;

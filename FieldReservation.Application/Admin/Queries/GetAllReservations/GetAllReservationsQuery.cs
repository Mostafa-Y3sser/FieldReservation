using FieldReservation.Application.Common.Results;
using MediatR;

namespace FieldReservation.Application.Admin.Queries.GetAllReservations;

public record GetAllReservationsQuery 
: IRequest<Result<List<ReservationDto>>>;
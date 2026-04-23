using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FieldReservation.Application.Common.Results;
using FieldReservation.Application.Common.Interfaces;

namespace FieldReservation.Application.Reservations.Queries.GetReservation
{
    public sealed class GetReservationQueryHandler(IAppDbContext context, ICurrentUserService currentUserService)
        : IRequestHandler<GetReservationQuery, Result<ReservationResponse>>
    {
        public async Task<Result<ReservationResponse>> Handle(
            GetReservationQuery request,
            CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;
            if (userId == null)
                return Error.Unauthorized();

            var reservation = await context.Reservations
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == request.ReservationId, cancellationToken);

            if (reservation is null)
                return Error.NotFound(description: "The reservation was not found.");

            if (reservation.UserId != userId)
                return Error.Forbidden(description: "You do not have permission to access this reservation.");

            return new ReservationResponse(
                reservation.Id,
                reservation.UserId,
                reservation.StartTime,
                reservation.EndTime,
                reservation.Status.ToString());
        }
    }
}
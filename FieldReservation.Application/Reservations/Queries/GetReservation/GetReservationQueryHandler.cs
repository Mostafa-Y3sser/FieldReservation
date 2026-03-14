using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FieldReservation.Application.Common.Results;
using FieldReservation.Application.Common.Interfaces;

namespace FieldReservation.Application.Reservations.Queries.GetReservation
{
    public sealed class GetReservationQueryHandler(IAppDbContext dbContext)
        : IRequestHandler<GetReservationQuery, Result<ReservationResponse>>
    {
        public async Task<Result<ReservationResponse>> Handle(
            GetReservationQuery request,
            CancellationToken cancellationToken)
        {
            var reservation = await dbContext.Reservations
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

            if (reservation is null)
                return Error.NotFound("Reservation.NotFound", "The reservation was not found.");

            var response = reservation.Adapt<ReservationResponse>();
            return response;
        }
    }
}
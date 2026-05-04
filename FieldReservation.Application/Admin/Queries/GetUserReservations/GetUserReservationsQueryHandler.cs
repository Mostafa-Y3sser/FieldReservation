using FieldReservation.Application.Admin.Queries.GetAllReservations;
using FieldReservation.Application.Common.Interfaces;
using FieldReservation.Application.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FieldReservation.Application.Admin.Queries.GetUserReservations;

public class GetUserReservationsQueryHandler(IAppDbContext context)
    : IRequestHandler<GetUserReservationsQuery, Result<List<ReservationDto>>>
{
    public async Task<Result<List<ReservationDto>>> Handle(GetUserReservationsQuery request, CancellationToken cancellationToken)
    {
        var reservations = await context.Reservations
            .Include(r => r.User)
            .Where(r => r.UserId == request.UserId)
            .OrderByDescending(r => r.StartTime)
            .Select(r => new ReservationDto(
                r.Id,
                r.StartTime,
                r.EndTime,
                r.Status.ToString(),
                r.MaintenanceNote,
                r.User.FullName
            ))
            .ToListAsync(cancellationToken);

        return reservations;
    }
}

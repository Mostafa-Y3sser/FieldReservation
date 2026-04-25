using FieldReservation.Application.Common.Interfaces;
using FieldReservation.Application.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FieldReservation.Application.Admin.Queries.GetAllReservations;

public class GetAllReservationsQueryHandler(IAppDbContext context)
    : IRequestHandler<GetAllReservationsQuery, Result<List<ReservationDto>>>
{
    public async Task<Result<List<ReservationDto>>> Handle(GetAllReservationsQuery request,
     CancellationToken cancellationToken)
    {
        var reservations = await context.Reservations
            .AsNoTracking()
            .Include(r => r.User)
            .OrderByDescending(r => r.StartTime)
            .Select(r => new ReservationDto(
                r.Id,
                r.StartTime,
                r.EndTime,
                r.Status.ToString(),
                r.MaintenanceNote,
                r.User != null ? r.User.FullName : null
            ))
            .ToListAsync(cancellationToken);

        return reservations;
    }
}
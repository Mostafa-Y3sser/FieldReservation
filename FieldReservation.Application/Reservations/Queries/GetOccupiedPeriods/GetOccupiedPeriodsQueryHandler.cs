using MediatR;
using Microsoft.EntityFrameworkCore;
using FieldReservation.Application.Common.Interfaces;
using FieldReservation.Application.Common.Results;
using FieldReservation.Domain.Enums;

namespace FieldReservation.Application.Reservations.Queries.GetOccupiedPeriods
{
    public sealed class GetOccupiedPeriodsQueryHandler(IAppDbContext context)
        : IRequestHandler<GetOccupiedPeriodsQuery, Result<List<OccupiedPeriodResponse>>>
    {
        public async Task<Result<List<OccupiedPeriodResponse>>> Handle(GetOccupiedPeriodsQuery request, CancellationToken cancellationToken)
        {
            var date = request.Date.Date;
            var nextDate = date.AddDays(1);

            var occupiedPeriods = await context.Reservations
                .AsNoTracking()
                .Where(r =>
                    r.Status != ReservationStatus.Cancelled &&
                    r.StartTime < nextDate &&
                    r.EndTime > date)
                .Select(r => new OccupiedPeriodResponse(
                    r.StartTime,
                    r.EndTime,
                    r.Status == ReservationStatus.Maintenance,
                    r.MaintenanceNote))
                .OrderBy(r => r.StartTime)
                .ToListAsync(cancellationToken);

            return occupiedPeriods;
        }
    }
}
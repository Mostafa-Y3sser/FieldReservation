using MediatR;
using Microsoft.EntityFrameworkCore;
using FieldReservation.Application.Common.Interfaces;
using FieldReservation.Application.Common.Results;
using FieldReservation.Domain.Enums;

namespace FieldReservation.Application.Reservations.Queries.GetOccupiedPeriods
{
    public sealed class GetOccupiedPeriodsQueryHandler(IAppDbContext context, ICurrentUserService currentUserService)
        : IRequestHandler<GetOccupiedPeriodsQuery, Result<List<OccupiedPeriodResponse>>>
    {
        public async Task<Result<List<OccupiedPeriodResponse>>> Handle(GetOccupiedPeriodsQuery request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;
            if (userId == null)
                return Error.Unauthorized();

            var user = await context.Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (user is null)
                return Error.Unauthorized(description: "User not found.");

            var date = request.Date.Date;
            var nextDate = date.AddDays(1);

            var occupiedPeriods = await context.Reservations
                .AsNoTracking()
                .Where(r =>
                    r.Status != ReservationStatus.Cancelled &&
                    r.StartTime < nextDate &&
                    r.EndTime > date)
                .OrderBy(r => r.StartTime)
                .Select(r => new OccupiedPeriodResponse(
                    user.FullName,
                    r.StartTime,
                    r.EndTime,
                    r.Status.ToString()))
                .ToListAsync(cancellationToken);

            return occupiedPeriods;
        }
    }
}
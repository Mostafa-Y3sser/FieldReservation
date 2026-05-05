using FieldReservation.Application.Common.Interfaces;
using FieldReservation.Application.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FieldReservation.Application.Reservations.Queries.GetMyReservations;

public class GetMyReservationsQueryHandler(IAppDbContext context, ICurrentUserService currentUserService)
    : IRequestHandler<GetMyReservationsQuery, Result<List<MyReservationResponse>>>
{
    public async Task<Result<List<MyReservationResponse>>> Handle(GetMyReservationsQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;

        if (string.IsNullOrEmpty(userId))
            return Error.Unauthorized();

        var reservations = await context.Reservations
            .AsNoTracking()
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.StartTime)
            .Select(r => new MyReservationResponse(
                r.Id,
                r.StartTime,
                r.EndTime,
                r.Status.ToString()
            ))
            .ToListAsync(cancellationToken);

        return reservations;
    }
}

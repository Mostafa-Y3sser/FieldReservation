using FieldReservation.Application.Common.Interfaces;
using FieldReservation.Application.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FieldReservation.Application.Admin.Queries.GetReservationDetails;

public sealed class GetReservationDetailsQueryHandler(IAppDbContext context)
    : IRequestHandler<GetReservationDetailsQuery, Result<AdminReservationDetailsResponse>>
{
    public async Task<Result<AdminReservationDetailsResponse>> Handle(GetReservationDetailsQuery request, CancellationToken cancellationToken)
    {
        var reservation = await context.Reservations
            .Include(r => r.User)
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == request.ReservationId, cancellationToken);

        if (reservation == null)
            return Error.NotFound(description: "Reservation not found.");

        return new AdminReservationDetailsResponse(
            reservation.Id,
            reservation.StartTime,
            reservation.EndTime,
            reservation.Status.ToString(),
            reservation.UserId,
            reservation.User?.FullName ?? "Unknown",
            reservation.User?.Email ?? "Unknown",
            reservation.User?.PhoneNumber,
            reservation.MaintenanceNote);
    }
}
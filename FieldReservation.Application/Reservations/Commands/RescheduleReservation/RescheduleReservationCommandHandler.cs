using MediatR;
using FieldReservation.Application.Common.Interfaces;
using FieldReservation.Application.Common.Results;
using Microsoft.EntityFrameworkCore;
using FieldReservation.Domain.Enums;

namespace FieldReservation.Application.Reservations.Commands.RescheduleReservation
{
    public sealed class RescheduleReservationCommandHandler(IAppDbContext context, ICurrentUserService currentUserService)
        : IRequestHandler<RescheduleReservationCommand, Result>
    {
        public async Task<Result> Handle(RescheduleReservationCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;
            if (userId == null)
                return Error.Unauthorized();

            // 1. Fetch Reservation
            var reservation = await context.Reservations
                .FirstOrDefaultAsync(r => r.Id == request.Id && r.UserId == userId
                , cancellationToken);

            if (reservation == null)
                return Error.NotFound(description: "The reservation was not found.");

            // 2. Status
            if (reservation.Status == ReservationStatus.Cancelled)
                return Error.Failure(description: "Cannot reschedule a cancelled reservation.");

            // 3. Buffer: 12 hours
            if (reservation.StartTime - DateTime.Now < TimeSpan.FromHours(12))
                return Error.Failure(description: "Reservations can only be rescheduled at least 12 hours before the start time.");

            // 4. Overlap Check for New Time
            var overlapExists = await context.Reservations
                .AnyAsync(r =>
                    r.Id != reservation.Id &&
                    r.FieldId == reservation.FieldId &&
                    r.Status != ReservationStatus.Cancelled &&
                    request.StartTime < r.EndTime &&
                    request.EndTime > r.StartTime,
                    cancellationToken);

            if (overlapExists)
                return Error.Conflict(description: "The new requested time slot is already occupied.");

            // 5. Update
            reservation.Reschedule(request.StartTime, request.EndTime);

            try
            {
                await context.SaveChangesAsync(cancellationToken);
                return Result.Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Error.Conflict(description: "The reservation was updated by someone else. Please refresh and try again.");
            }
        }
    }
}
using MediatR;
using Microsoft.EntityFrameworkCore;
using FieldReservation.Application.Common.Interfaces;
using FieldReservation.Application.Common.Results;
using FieldReservation.Domain.Entities;
using FieldReservation.Domain.Enums;

namespace FieldReservation.Application.Reservations.Commands.CreateReservation
{
    public sealed class CreateReservationCommandHandler(IAppDbContext context, ICurrentUserService currentUserService)
        : IRequestHandler<CreateReservationCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;
            if (userId == null)
                return Error.Unauthorized();

            // 1. Fetch the single "Elite Turf" field
            var field = await context.Fields
                .FirstOrDefaultAsync(f => f.Name == "Elite Turf", cancellationToken);

            if (field == null)
                return Error.Failure(description: "The 'Elite Turf' field has not been initialized.");

            // 2. Check for overlaps
            var overlapExists = await context.Reservations
                .AnyAsync(r =>
                    r.FieldId == field.Id &&
                    r.Status != ReservationStatus.Cancelled &&
                    request.StartTime < r.EndTime &&
                    request.EndTime > r.StartTime,
                    cancellationToken);

            if (overlapExists)
                return Error.Conflict(description: "The requested time slot is already occupied.");

            // 3. Create and Save
            var reservation = Reservation.Create(
                field.Id,
                userId,
                request.StartTime,
                request.EndTime);

            context.Reservations.Add(reservation);

            try
            {
                await context.SaveChangesAsync(cancellationToken);
                return reservation.Id;
            }
            catch (DbUpdateConcurrencyException)
            {
                return Error.Conflict(description: "The reservation could not be completed because the slot was booked by someone else.");
            }
        }
    }
}
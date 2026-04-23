using MediatR;
using FieldReservation.Application.Common.Interfaces;
using FieldReservation.Application.Common.Results;
using FieldReservation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using FieldReservation.Domain.Enums;

namespace FieldReservation.Application.Admin.Commands.CreateMaintenance
{
    public sealed class CreateMaintenanceCommandHandler(IAppDbContext context)
        : IRequestHandler<CreateMaintenanceCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(CreateMaintenanceCommand request, CancellationToken cancellationToken)
        {
            var field = await context.Fields.FirstOrDefaultAsync(f => f.Name == "Elite Turf", cancellationToken);

            if (field == null)
                return Error.Failure(description: "The 'Elite Turf' field has not been initialized.");

            // Overlap check
            var overlapExists = await context.Reservations
                .AnyAsync(r =>
                    r.FieldId == field.Id &&
                    r.Status != ReservationStatus.Cancelled &&
                    request.StartTime < r.EndTime &&
                    request.EndTime > r.StartTime,
                    cancellationToken);

            if (overlapExists)
                return Error.Conflict(description: "The maintenance slot overlaps with an existing reservation.");

            var maintenance = Reservation.CreateMaintenance(field.Id, request.StartTime, request.EndTime, request.Note);
            context.Reservations.Add(maintenance);

            try
            {
                await context.SaveChangesAsync(cancellationToken);
                return maintenance.Id;
            }
            catch (DbUpdateConcurrencyException)
            {
                return Error.Conflict(description: "The maintenance slot could not be booked because the slot was booked by someone else.");
            }
        }
    }
}
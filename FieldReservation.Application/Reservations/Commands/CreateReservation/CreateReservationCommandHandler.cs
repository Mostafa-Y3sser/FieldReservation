using MediatR;
using Microsoft.EntityFrameworkCore;
using FieldReservation.Domain.Entities;
using FieldReservation.Application.Common.Results;
using FieldReservation.Application.Common.Interfaces;

namespace FieldReservation.Application.Reservations.Commands.CreateReservation
{

    public sealed class CreateReservationCommandHandler(IAppDbContext dbContext)
        : IRequestHandler<CreateReservationCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(
            CreateReservationCommand request,
            CancellationToken cancellationToken)
        {
            var hasConflict = await dbContext.Reservations
                .AsNoTracking()
                .AnyAsync(
                    r => r.FieldId == request.FieldId &&
                         r.StartTime < request.EndTime &&
                         r.EndTime > request.StartTime,
                    cancellationToken);

            if (hasConflict)
                return Error.Failure("Reservation.TimeConflict", "The field is already reserved for this time slot.");

            var reservation = Reservation.Create(
                request.FieldId,
                request.UserId,
                request.StartTime,
                request.EndTime);

            await dbContext.Reservations.AddAsync(reservation, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return reservation.Id;
        }
    }
}
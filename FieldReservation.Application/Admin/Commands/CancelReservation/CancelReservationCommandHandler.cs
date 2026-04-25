using MediatR;
using FieldReservation.Application.Common.Interfaces;
using FieldReservation.Application.Common.Results;
using Microsoft.EntityFrameworkCore;
using FieldReservation.Domain.Enums;

namespace FieldReservation.Application.Admin.Commands.CancelReservation
{
    public sealed class CancelReservationCommandHandler(IAppDbContext context)
        : IRequestHandler<CancelReservationCommand, Result>
    {
        public async Task<Result> Handle(CancelReservationCommand request, CancellationToken cancellationToken)
        {
            var reservation = await context.Reservations
                .FirstOrDefaultAsync(r => r.Id == request.ReservationId, cancellationToken);

            if (reservation == null)
                return Error.NotFound(description: "The reservation was not found.");

            if (request.NewStatus == ReservationStatus.Cancelled)
                reservation.Cancel();
            else
                // Logic for status overrides can be expanded here
                return Error.Failure(description: "Status override for this type is not yet supported.");

            await context.SaveChangesAsync(cancellationToken);
            return Result.Ok();
        }
    }
}
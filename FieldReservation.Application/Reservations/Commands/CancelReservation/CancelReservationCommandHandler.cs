using MediatR;
using FieldReservation.Application.Common.Interfaces;
using FieldReservation.Application.Common.Results;
using Microsoft.EntityFrameworkCore;
using FieldReservation.Domain.Enums;

namespace FieldReservation.Application.Reservations.Commands.CancelReservation
{
    public sealed class CancelReservationCommandHandler(IAppDbContext context, ICurrentUserService currentUserService)
        : IRequestHandler<CancelReservationCommand, Result>
    {
        public async Task<Result> Handle(CancelReservationCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;
            if (userId == null)
                return Error.Unauthorized();

            var reservation = await context.Reservations
                .FirstOrDefaultAsync(r => r.Id == request.Id && r.UserId == userId
                , cancellationToken);

            if (reservation == null)
                return Error.NotFound(description: "The reservation was not found.");

            if (reservation.Status == ReservationStatus.Cancelled)
                return Error.Failure(description: "The reservation is already cancelled.");

            // Cancellation buffer: 6 hours
            if (reservation.StartTime - DateTime.Now < TimeSpan.FromHours(6))
                return Error.Failure(description: "Reservations can only be cancelled at least 6 hours in advance.");

            reservation.Cancel();
            await context.SaveChangesAsync(cancellationToken);

            return Result.Ok();
        }
    }
}
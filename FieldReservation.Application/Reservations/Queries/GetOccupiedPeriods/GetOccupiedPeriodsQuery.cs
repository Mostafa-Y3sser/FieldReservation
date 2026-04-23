using MediatR;
using FieldReservation.Application.Common.Results;

namespace FieldReservation.Application.Reservations.Queries.GetOccupiedPeriods
{
    public record GetOccupiedPeriodsQuery(
        DateTime Date) : IRequest<Result<List<OccupiedPeriodResponse>>>;
}
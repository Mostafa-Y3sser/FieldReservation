using MediatR;
using FieldReservation.Application.Common.Results;

namespace FieldReservation.Application.Admin.Commands.CreateMaintenance
{
    public record CreateMaintenanceCommand(
     DateTime StartTime,
     DateTime EndTime,
     string Note) : IRequest<Result<Guid>>;
}
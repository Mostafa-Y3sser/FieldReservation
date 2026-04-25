using FieldReservation.API.Common;
using FieldReservation.Application.Admin.Commands.BlockUser;
using FieldReservation.Application.Admin.Commands.CancelReservation;
using FieldReservation.Application.Admin.Commands.CreateMaintenance;
using FieldReservation.Application.Admin.Queries.GetAllReservations;
using FieldReservation.Application.Admin.Queries.GetReservationDetails;
using FieldReservation.Application.Common.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FieldReservation.API.Controllers
{
    [Route("api/admin/reservations")]
    [Authorize(Roles = Roles.Owner)]
    public class AdminReservationsController(ISender sender) : BaseApiController
    {
        [HttpGet]
        [ProducesResponseType(typeof(List<ReservationDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await sender.Send(new GetAllReservationsQuery(), cancellationToken);
            return HandleResult<List<ReservationDto>>(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AdminReservationDetailsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await sender.Send(new GetReservationDetailsQuery(id), cancellationToken);
            return HandleResult<AdminReservationDetailsResponse>(result);
        }

        [HttpPost("maintenance")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateMaintenance(
            [FromBody] CreateMaintenanceCommand command,
            CancellationToken cancellationToken)
        {
            var result = await sender.Send(command, cancellationToken);
            return HandleResult<Guid>(result);
        }

        [HttpPatch("cancel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CancelReservation(
            [FromBody] CancelReservationCommand command,
            CancellationToken cancellationToken)
        {
            var result = await sender.Send(command, cancellationToken);
            return HandleResult(result);
        }
    }
}
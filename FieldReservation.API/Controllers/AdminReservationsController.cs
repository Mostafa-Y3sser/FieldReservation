using FieldReservation.API.Common;
using FieldReservation.Application.Admin.Commands.CreateMaintenance;
using FieldReservation.Application.Admin.Commands.OverrideReservation;
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
        [HttpPost("maintenance")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> BlockMaintenance(
            [FromBody] CreateMaintenanceCommand command,
            CancellationToken cancellationToken)
        {
            var result = await sender.Send(command, cancellationToken);

            if (!result.IsSucceeded)
                return HandleResult(result);

            return Created("", result.Value);
        }

        [HttpPatch("{id:guid}/override")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> OverrideReservation(
            Guid id,
            [FromBody] OverrideReservationCommand command,
            CancellationToken cancellationToken)
        {
            if (id != command.ReservationId) return BadRequest("ID mismatch");
            var result = await sender.Send(command, cancellationToken);
            return HandleResult(result);
        }
    }
}

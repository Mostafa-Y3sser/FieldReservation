using FieldReservation.API.Common;
using FieldReservation.Application.Reservations.Commands.CancelReservation;
using FieldReservation.Application.Reservations.Commands.CreateReservation;
using FieldReservation.Application.Reservations.Commands.RescheduleReservation;
using FieldReservation.Application.Reservations.Queries.GetMyReservations;
using FieldReservation.Application.Reservations.Queries.GetOccupiedPeriods;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FieldReservation.API.Controllers;

[Route("api/[controller]")]
[Authorize]
public class ReservationsController(ISender sender) : BaseApiController
{
    /// <summary>Creates a new reservation and returns a Stripe checkout URL for payment.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(CreateReservationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] CreateReservationCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return HandleResult<CreateReservationResponse>(result);
    }

    /// <summary>Cancels a reservation.</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new CancelReservationCommand(id), cancellationToken);
        return HandleResult(result);
    }

    /// <summary>Reschedules a reservation.</summary>
    [HttpPatch("reschedule")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Reschedule(
        [FromBody] RescheduleReservationCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return HandleResult(result);
    }

    /// <summary>Gets occupied periods for a date.</summary>
    [HttpGet("occupied")]
    [ProducesResponseType(typeof(List<OccupiedPeriodResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOccupiedPeriods([FromQuery] DateTime date, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetOccupiedPeriodsQuery(date), cancellationToken);
        return HandleResult(result);
    }

    /// <summary>Gets all reservations for the current user.</summary>
    [HttpGet("my-reservations")]
    [ProducesResponseType(typeof(List<MyReservationResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyReservations(CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetMyReservationsQuery(), cancellationToken);
        return HandleResult<List<MyReservationResponse>>(result);
    }
}
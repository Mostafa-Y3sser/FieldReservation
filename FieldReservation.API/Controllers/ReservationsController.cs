using FieldReservation.API.Common;
using FieldReservation.Application.Reservations.Commands.CreateReservation;
using FieldReservation.Application.Reservations.Queries.GetReservation;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FieldReservation.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class ReservationsController(ISender sender) : BaseApiController
{
    /// <summary>Creates a new reservation.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateReservationCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        
        if (!result.IsSucceeded)
            return HandleResult<Guid>(result);

        return CreatedAtAction(nameof(GetById), new { id = result.Value, version = "1.0" }, result.Value);
    }

    /// <summary>Gets a reservation by ID.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ReservationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetReservationQuery(id), cancellationToken);
        return HandleResult<ReservationResponse>(result);
    }
}


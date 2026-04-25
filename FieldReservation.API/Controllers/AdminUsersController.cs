using FieldReservation.API.Common;
using FieldReservation.Application.Admin.Commands.BlockUser;
using FieldReservation.Application.Admin.Queries.GetAllUsers;
using FieldReservation.Application.Common.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FieldReservation.API.Controllers
{
    [Route("api/admin/users")]
    [Authorize(Roles = Roles.Owner)]
    public class AdminUsersController(ISender sender) : BaseApiController
    {
        [HttpGet]
        [ProducesResponseType(typeof(List<UserResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await sender.Send(new GetAllUsersQuery(), cancellationToken);
            return HandleResult<List<UserResponse>>(result);
        }

        [HttpPost("{id}/block")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> BlockUser(string id, CancellationToken cancellationToken)
        {
            var result = await sender.Send(new BlockUserCommand(id), cancellationToken);
            return HandleResult(result);
        }
    }
}
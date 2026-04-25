using FieldReservation.Application.Common.Interfaces;
using FieldReservation.Application.Common.Results;
using MediatR;

namespace FieldReservation.Application.Admin.Commands.BlockUser;

public class BlockUserCommandHandler(IAuthService authService)
 : IRequestHandler<BlockUserCommand, Result>
{
    public async Task<Result> Handle(BlockUserCommand request, CancellationToken cancellationToken)
    {
        return await authService.BlockUserAsync(request.UserId);
    }
}
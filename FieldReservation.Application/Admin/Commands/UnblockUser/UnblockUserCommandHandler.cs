using FieldReservation.Application.Common.Interfaces;
using FieldReservation.Application.Common.Results;
using MediatR;

namespace FieldReservation.Application.Admin.Commands.UnblockUser;

public class UnblockUserCommandHandler(IAuthService authService)
    : IRequestHandler<UnblockUserCommand, Result>
{
    public async Task<Result> Handle(UnblockUserCommand request, CancellationToken cancellationToken)
    {
        return await authService.UnblockUserAsync(request.UserId);
    }
}

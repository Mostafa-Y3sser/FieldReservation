using FieldReservation.Application.Auth.Dtos;
using FieldReservation.Application.Common.Interfaces;
using FieldReservation.Application.Common.Results;
using MediatR;

namespace FieldReservation.Application.Auth.Queries.GetCurrentUser;

public class GetCurrentUserQueryHandler(IAuthService authService, ICurrentUserService currentUserService)
    : IRequestHandler<GetCurrentUserQuery, Result<UserProfileResponse>>
{
    public async Task<Result<UserProfileResponse>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;

        if (string.IsNullOrEmpty(userId))
            return Error.Unauthorized();

        return await authService.GetUserProfileAsync(userId);
    }
}

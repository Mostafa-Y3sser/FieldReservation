using FieldReservation.Application.Common.Interfaces;
using FieldReservation.Application.Common.Results;
using MediatR;

namespace FieldReservation.Application.Admin.Queries.GetAllUsers;

public sealed class GetAllUsersQueryHandler(IAuthService authService)
    : IRequestHandler<GetAllUsersQuery, Result<List<UserResponse>>>
{
    public async Task<Result<List<UserResponse>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var result = await authService.GetAllUsersAsync();

        if (result.IsFailed)
            return result.Errors.ToList();

        var userResponses = result.Value.Select(u => new UserResponse(
            u.Id,
            u.FullName,
            u.Email,
            u.PhoneNumber)).ToList();

        return userResponses;
    }
}
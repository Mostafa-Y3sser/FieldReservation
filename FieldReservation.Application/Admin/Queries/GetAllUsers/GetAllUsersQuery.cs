using FieldReservation.Application.Common.Results;
using MediatR;

namespace FieldReservation.Application.Admin.Queries.GetAllUsers;

public record GetAllUsersQuery : IRequest<Result<List<UserResponse>>>;

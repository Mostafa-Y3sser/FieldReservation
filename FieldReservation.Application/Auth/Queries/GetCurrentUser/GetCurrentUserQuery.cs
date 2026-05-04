using FieldReservation.Application.Auth.Dtos;
using FieldReservation.Application.Common.Results;
using MediatR;

namespace FieldReservation.Application.Auth.Queries.GetCurrentUser;

public record GetCurrentUserQuery : IRequest<Result<UserProfileResponse>>;

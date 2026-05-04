using FieldReservation.Application.Common.Results;
using MediatR;

namespace FieldReservation.Application.Admin.Commands.UnblockUser;

public record UnblockUserCommand(string UserId) : IRequest<Result>;

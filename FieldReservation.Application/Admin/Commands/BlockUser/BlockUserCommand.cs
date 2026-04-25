using FieldReservation.Application.Common.Results;
using MediatR;

namespace FieldReservation.Application.Admin.Commands.BlockUser;

public record BlockUserCommand(string UserId) 
: IRequest<Result>;
using Futvibe.Application.Common.DTOs;
using MediatR;

namespace Futvibe.Application.Users.Queries.GetUserById;

public record GetUserByIdQuery(Guid UserId) : IRequest<UserDto>;

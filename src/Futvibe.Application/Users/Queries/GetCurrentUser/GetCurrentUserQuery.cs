using Futvibe.Application.Common.DTOs;
using MediatR;

namespace Futvibe.Application.Users.Queries.GetCurrentUser;

public record GetCurrentUserQuery(Guid UserId) : IRequest<UserDto>;

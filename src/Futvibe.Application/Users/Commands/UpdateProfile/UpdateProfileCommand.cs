using Futvibe.Application.Common.DTOs;
using Futvibe.Domain.Enums;
using MediatR;

namespace Futvibe.Application.Users.Commands.UpdateProfile;

public record UpdateProfileCommand(
    Guid UserId,
    string Name,
    string? Bio,
    MatchLevel Level
) : IRequest<UserDto>;

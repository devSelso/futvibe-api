using Futvibe.Application.Common.DTOs;
using Futvibe.Domain.Entities;

namespace Futvibe.Application.Common.Mappers;

public static class UserMapper
{
    public static UserDto ToDto(User user) => new(
        user.Id,
        user.Name,
        user.Email,
        user.Avatar,
        user.Bio,
        user.Level.ToString().ToLower(),
        user.PresenceScore,
        user.MatchesPlayed);
}

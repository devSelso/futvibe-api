namespace Futvibe.Application.Common.DTOs;

public record UserDto(
    Guid Id,
    string Name,
    string Email,
    string? Avatar,
    string? Bio,
    string Level,
    int PresenceScore,
    int MatchesPlayed
);

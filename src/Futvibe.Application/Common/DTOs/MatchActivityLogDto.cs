namespace Futvibe.Application.Common.DTOs;

public record MatchActivityLogDto(
    Guid Id,
    Guid UserId,
    string UserName,
    string? UserAvatar,
    string Action,
    DateTime CreatedAt
);

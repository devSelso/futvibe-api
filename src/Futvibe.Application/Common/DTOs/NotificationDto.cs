using Futvibe.Domain.Enums;

namespace Futvibe.Application.Common.DTOs;

public record NotificationDto(
    Guid Id,
    NotificationType Type,
    Guid? MatchId,
    string Message,
    bool IsRead,
    DateTime CreatedAt
);

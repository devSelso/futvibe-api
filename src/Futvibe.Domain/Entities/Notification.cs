using Futvibe.Domain.Enums;

namespace Futvibe.Domain.Entities;

public class Notification
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public NotificationType Type { get; private set; }
    public Guid? MatchId { get; private set; }
    public string Message { get; private set; } = default!;
    public bool IsRead { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Notification() { }

    public static Notification Create(Guid userId, NotificationType type, Guid? matchId, string message)
        => new()
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Type = type,
            MatchId = matchId,
            Message = message,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

    public void MarkAsRead() => IsRead = true;
}

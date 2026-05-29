using Futvibe.Domain.Enums;

namespace Futvibe.Domain.Entities;

public class MatchActivityLog
{
    public Guid Id { get; private set; }
    public Guid MatchId { get; private set; }
    public Guid UserId { get; private set; }
    public MatchActivityAction Action { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public User? User { get; private set; }

    private MatchActivityLog() { }

    public static MatchActivityLog Create(Guid matchId, Guid userId, MatchActivityAction action)
        => new()
        {
            Id = Guid.NewGuid(),
            MatchId = matchId,
            UserId = userId,
            Action = action,
            CreatedAt = DateTime.UtcNow
        };
}

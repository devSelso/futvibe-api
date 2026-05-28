using Futvibe.Domain.Enums;

namespace Futvibe.Domain.Entities;

public class Participant
{
    public Guid MatchId { get; private set; }
    public Guid UserId { get; private set; }
    public ParticipantStatus Status { get; private set; }

    public User? User { get; private set; }

    private Participant() { }

    public static Participant Create(Guid matchId, Guid userId, ParticipantStatus status)
    {
        return new Participant
        {
            MatchId = matchId,
            UserId = userId,
            Status = status
        };
    }

    public void UpdateStatus(ParticipantStatus status) => Status = status;
}

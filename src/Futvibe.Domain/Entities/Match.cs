using Futvibe.Domain.Enums;
using Futvibe.Domain.Exceptions;

namespace Futvibe.Domain.Entities;

public class Match
{
    public Guid Id { get; private set; }
    public string Title { get; private set; } = default!;
    public string Location { get; private set; } = default!;
    public DateOnly Date { get; private set; }
    public TimeOnly Time { get; private set; }
    public MatchLevel Level { get; private set; }
    public decimal PricePerPlayer { get; private set; }
    public int MaxPlayers { get; private set; }
    public MatchVisibility Visibility { get; private set; }
    public Guid HostId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public IReadOnlyCollection<Participant> Participants => _participants.AsReadOnly();
    private readonly List<Participant> _participants = [];

    private Match() { }

    public static Match Create(
        string title,
        string location,
        DateOnly date,
        TimeOnly time,
        MatchLevel level,
        decimal pricePerPlayer,
        int maxPlayers,
        MatchVisibility visibility,
        Guid hostId)
    {
        var match = new Match
        {
            Id = Guid.NewGuid(),
            Title = title,
            Location = location,
            Date = date,
            Time = time,
            Level = level,
            PricePerPlayer = pricePerPlayer,
            MaxPlayers = maxPlayers,
            Visibility = visibility,
            HostId = hostId,
            CreatedAt = DateTime.UtcNow
        };

        match._participants.Add(Participant.Create(match.Id, hostId, ParticipantStatus.Host));

        return match;
    }

    public ParticipantStatus DetermineStatusForNewJoiner()
    {
        var confirmedCount = _participants.Count(p =>
            p.Status is ParticipantStatus.Confirmed or ParticipantStatus.Host);

        if (confirmedCount >= MaxPlayers)
            return ParticipantStatus.Waitlist;

        return Visibility switch
        {
            MatchVisibility.Private => ParticipantStatus.Confirmed,
            MatchVisibility.Hybrid  => ParticipantStatus.Confirmed,
            MatchVisibility.Public  => ParticipantStatus.Pending,
            _                       => ParticipantStatus.Pending
        };
    }

    public void AddParticipant(Guid userId, ParticipantStatus status)
    {
        if (_participants.Any(p => p.UserId == userId))
            throw new BusinessException("User already in match.");

        _participants.Add(Participant.Create(Id, userId, status));
    }

    public void UpdateParticipantStatus(Guid requestingUserId, Guid targetUserId, ParticipantStatus newStatus)
    {
        if (HostId != requestingUserId)
            throw new ForbiddenException("Only the host can update participant status.");

        var participant = _participants.FirstOrDefault(p => p.UserId == targetUserId)
            ?? throw new NotFoundException($"Participant {targetUserId} not found in match.");

        participant.UpdateStatus(newStatus);
    }
}

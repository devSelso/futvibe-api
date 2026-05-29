using Futvibe.Domain.Enums;
using Futvibe.Domain.Exceptions;

namespace Futvibe.Domain.Entities;

public class Match
{
    private const int ValidationWindowHours = 72;

    public Guid Id { get; private set; }
    public string Title { get; private set; } = default!;
    public string Location { get; private set; } = default!;
    public DateOnly Date { get; private set; }
    public TimeOnly Time { get; private set; }
    public MatchLevel Level { get; private set; }
    public decimal PricePerPlayer { get; private set; }
    public int MaxPlayers { get; private set; }
    public MatchVisibility Visibility { get; private set; }
    public MatchStatus Status { get; private set; } = MatchStatus.Scheduled;
    public Guid HostId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public IReadOnlyCollection<Participant> Participants => _participants.AsReadOnly();
    private readonly List<Participant> _participants = [];

    private Match() { }

    public static Match Create(
        string title, string location,
        DateOnly date, TimeOnly time,
        MatchLevel level, decimal pricePerPlayer,
        int maxPlayers, MatchVisibility visibility,
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
            Status = MatchStatus.Scheduled,
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
        var existing = _participants.FirstOrDefault(p => p.UserId == userId);
        if (existing is not null)
        {
            if (existing.Status == ParticipantStatus.Left)
                throw new BusinessException("Você já desistiu desta partida.");
            throw new BusinessException("Você já está nesta partida.");
        }

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

    public void Cancel(Guid requestingUserId)
    {
        if (HostId != requestingUserId)
            throw new ForbiddenException("Apenas o host pode cancelar a partida.");

        if (Status is MatchStatus.Closed or MatchStatus.Cancelled)
            throw new BusinessException("Não é possível cancelar uma partida já encerrada ou cancelada.");

        Status = MatchStatus.Cancelled;
    }

    public void ValidateCanDelete(Guid requestingUserId)
    {
        if (HostId != requestingUserId)
            throw new ForbiddenException("Apenas o host pode excluir a partida.");

        if (Status != MatchStatus.Scheduled)
            throw new BusinessException("Não é possível excluir uma partida já encerrada ou em validação.");
    }

    public void Edit(
        Guid requestingUserId,
        string title, string location,
        DateOnly date, TimeOnly time,
        MatchLevel level, decimal pricePerPlayer,
        int maxPlayers, MatchVisibility visibility)
    {
        if (HostId != requestingUserId)
            throw new ForbiddenException("Apenas o host pode editar a partida.");

        if (Status != MatchStatus.Scheduled)
            throw new BusinessException("Não é possível editar uma partida já encerrada ou em validação.");

        Title = title;
        Location = location;
        Date = date;
        Time = time;
        Level = level;
        PricePerPlayer = pricePerPlayer;
        MaxPlayers = maxPlayers;
        Visibility = visibility;
    }

    public void RemoveParticipant(Guid userId)
    {
        if (userId == HostId)
            throw new ForbiddenException("O host não pode desistir da própria partida.");

        if (Status != MatchStatus.Scheduled)
            throw new BusinessException("Não é possível desistir de uma partida já encerrada ou em validação.");

        var participant = _participants.FirstOrDefault(p => p.UserId == userId)
            ?? throw new NotFoundException($"Participante {userId} não encontrado na partida.");

        if (participant.Status == ParticipantStatus.Left)
            throw new BusinessException("Você já desistiu desta partida.");

        participant.UpdateStatus(ParticipantStatus.Left);
    }

    // Returns participants eligible for presence validation (Confirmed only)
    public IReadOnlyList<(Guid UserId, bool Present)> ValidatePresence(
        Guid requestingUserId,
        IEnumerable<(Guid UserId, bool Present)> validations)
    {
        if (HostId != requestingUserId)
            throw new ForbiddenException("Apenas o host pode validar presença.");

        if (Status == MatchStatus.Closed)
            throw new BusinessException("Partida já encerrada.");

        if (Status != MatchStatus.PendingValidation)
            throw new BusinessException("A janela de validação ainda não está disponível.");

        Status = MatchStatus.Closed;

        return validations
            .Where(v => _participants.Any(p => p.UserId == v.UserId && p.Status == ParticipantStatus.Confirmed))
            .ToList();
    }

    // Lazy maintenance — called on GET to keep status consistent
    public bool TryAdvanceStatus()
    {
        if (Status is MatchStatus.Closed or MatchStatus.Cancelled) return false;

        var matchDateTime = Date.ToDateTime(Time, DateTimeKind.Utc);
        var now = DateTime.UtcNow;

        if (Status == MatchStatus.Scheduled && now >= matchDateTime)
        {
            Status = now <= matchDateTime.AddHours(ValidationWindowHours)
                ? MatchStatus.PendingValidation
                : MatchStatus.Closed;
            return true;
        }

        if (Status == MatchStatus.PendingValidation && now > matchDateTime.AddHours(ValidationWindowHours))
        {
            Status = MatchStatus.Closed;
            return true;
        }

        return false;
    }
}

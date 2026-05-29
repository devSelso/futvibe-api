namespace Futvibe.Application.Common.DTOs;

public record MatchDto(
    Guid Id,
    string Title,
    string Location,
    string Date,
    string Time,
    string Level,
    decimal PricePerPlayer,
    int MaxPlayers,
    string Visibility,
    string Status,
    Guid HostId,
    int ParticipantCount,
    IReadOnlyList<ParticipantDto> Participants
);

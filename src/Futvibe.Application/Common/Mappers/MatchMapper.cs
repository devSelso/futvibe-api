using Futvibe.Application.Common.DTOs;
using Futvibe.Domain.Entities;

namespace Futvibe.Application.Common.Mappers;

public static class MatchMapper
{
    public static MatchDto ToDto(Match match) => new(
        match.Id,
        match.Title,
        match.Location,
        match.Date.ToString("yyyy-MM-dd"),
        match.Time.ToString("HH:mm"),
        match.Level.ToString().ToLower(),
        match.PricePerPlayer,
        match.MaxPlayers,
        match.Visibility.ToString().ToLower(),
        match.HostId,
        match.Participants.Select(p => new ParticipantDto(
            p.UserId,
            p.Status.ToString().ToLower(),
            p.User is null ? null : new UserDto(
                p.User.Id,
                p.User.Name,
                p.User.Email,
                p.User.Avatar,
                p.User.Bio,
                p.User.Level.ToString().ToLower(),
                p.User.PresenceScore,
                p.User.MatchesPlayed)
        )).ToList()
    );
}

namespace Futvibe.Application.Common.DTOs;

public record ParticipantDto(
    Guid UserId,
    string Status,
    UserDto? User
);

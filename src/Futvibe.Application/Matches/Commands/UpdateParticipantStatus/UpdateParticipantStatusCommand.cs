using Futvibe.Domain.Enums;
using MediatR;

namespace Futvibe.Application.Matches.Commands.UpdateParticipantStatus;

public record UpdateParticipantStatusCommand(
    Guid MatchId,
    Guid TargetUserId,
    Guid RequestingUserId,
    ParticipantStatus NewStatus
) : IRequest;

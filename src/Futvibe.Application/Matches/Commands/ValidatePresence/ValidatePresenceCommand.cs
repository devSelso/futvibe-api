using MediatR;

namespace Futvibe.Application.Matches.Commands.ValidatePresence;

public record ParticipantPresence(Guid UserId, bool Present);

public record ValidatePresenceCommand(
    Guid MatchId,
    Guid RequestingUserId,
    IReadOnlyList<ParticipantPresence> Validations
) : IRequest;

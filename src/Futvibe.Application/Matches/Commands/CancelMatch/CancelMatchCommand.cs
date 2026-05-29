using MediatR;

namespace Futvibe.Application.Matches.Commands.CancelMatch;

public record CancelMatchCommand(Guid MatchId, Guid RequestingUserId) : IRequest;

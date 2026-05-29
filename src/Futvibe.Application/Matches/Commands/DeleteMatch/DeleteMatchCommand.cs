using MediatR;

namespace Futvibe.Application.Matches.Commands.DeleteMatch;

public record DeleteMatchCommand(Guid MatchId, Guid RequestingUserId) : IRequest;

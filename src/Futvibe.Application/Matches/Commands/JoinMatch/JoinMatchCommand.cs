using MediatR;

namespace Futvibe.Application.Matches.Commands.JoinMatch;

public record JoinMatchCommand(Guid MatchId, Guid RequestingUserId) : IRequest;

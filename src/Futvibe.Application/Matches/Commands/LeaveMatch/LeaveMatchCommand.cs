using MediatR;

namespace Futvibe.Application.Matches.Commands.LeaveMatch;

public record LeaveMatchCommand(Guid MatchId, Guid UserId) : IRequest;

using Futvibe.Domain.Exceptions;
using Futvibe.Domain.Interfaces.Repositories;
using MediatR;

namespace Futvibe.Application.Matches.Commands.JoinMatch;

public class JoinMatchCommandHandler(IMatchRepository matchRepo)
    : IRequestHandler<JoinMatchCommand>
{
    public async Task Handle(JoinMatchCommand request, CancellationToken ct)
    {
        var match = await matchRepo.GetByIdWithParticipantsAsync(request.MatchId, ct)
            ?? throw new NotFoundException($"Partida {request.MatchId} não encontrada.");

        var status = match.DetermineStatusForNewJoiner();
        match.AddParticipant(request.RequestingUserId, status);

        await matchRepo.SaveChangesAsync(ct);
    }
}

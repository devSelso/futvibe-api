using Futvibe.Domain.Exceptions;
using Futvibe.Domain.Interfaces.Repositories;
using MediatR;

namespace Futvibe.Application.Matches.Commands.DeleteMatch;

public class DeleteMatchCommandHandler(IMatchRepository matchRepo)
    : IRequestHandler<DeleteMatchCommand>
{
    public async Task Handle(DeleteMatchCommand request, CancellationToken ct)
    {
        var match = await matchRepo.GetByIdWithParticipantsAsync(request.MatchId, ct)
            ?? throw new NotFoundException($"Partida {request.MatchId} não encontrada.");

        match.ValidateCanDelete(request.RequestingUserId);

        matchRepo.Delete(match);
        await matchRepo.SaveChangesAsync(ct);
    }
}

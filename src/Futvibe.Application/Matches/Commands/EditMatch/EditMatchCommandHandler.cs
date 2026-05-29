using Futvibe.Domain.Exceptions;
using Futvibe.Domain.Interfaces.Repositories;
using MediatR;

namespace Futvibe.Application.Matches.Commands.EditMatch;

public class EditMatchCommandHandler(IMatchRepository matchRepo)
    : IRequestHandler<EditMatchCommand>
{
    public async Task Handle(EditMatchCommand request, CancellationToken ct)
    {
        var match = await matchRepo.GetByIdWithParticipantsAsync(request.MatchId, ct)
            ?? throw new NotFoundException($"Partida {request.MatchId} não encontrada.");

        match.Edit(
            request.RequestingUserId,
            request.Title, request.Location,
            request.Date, request.Time,
            request.Level, request.PricePerPlayer,
            request.MaxPlayers, request.Visibility);

        await matchRepo.SaveChangesAsync(ct);
    }
}

using Futvibe.Domain.Exceptions;
using Futvibe.Domain.Interfaces.Repositories;
using MediatR;

namespace Futvibe.Application.Matches.Commands.UpdateParticipantStatus;

public class UpdateParticipantStatusCommandHandler(IMatchRepository matchRepo)
    : IRequestHandler<UpdateParticipantStatusCommand>
{
    public async Task Handle(UpdateParticipantStatusCommand request, CancellationToken ct)
    {
        var match = await matchRepo.GetByIdWithParticipantsAsync(request.MatchId, ct)
            ?? throw new NotFoundException($"Partida {request.MatchId} não encontrada.");

        match.UpdateParticipantStatus(request.RequestingUserId, request.TargetUserId, request.NewStatus);

        await matchRepo.SaveChangesAsync(ct);
    }
}

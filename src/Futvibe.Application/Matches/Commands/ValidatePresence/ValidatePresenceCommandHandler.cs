using Futvibe.Domain.Exceptions;
using Futvibe.Domain.Interfaces.Repositories;
using MediatR;

namespace Futvibe.Application.Matches.Commands.ValidatePresence;

public class ValidatePresenceCommandHandler(
    IMatchRepository matchRepo,
    IUserRepository userRepo) : IRequestHandler<ValidatePresenceCommand>
{
    public async Task Handle(ValidatePresenceCommand request, CancellationToken ct)
    {
        var match = await matchRepo.GetByIdWithParticipantsAsync(request.MatchId, ct)
            ?? throw new NotFoundException($"Partida {request.MatchId} não encontrada.");

        var validationPairs = request.Validations
            .Select(v => (v.UserId, v.Present))
            .AsEnumerable();

        var eligibleValidations = match.ValidatePresence(request.RequestingUserId, validationPairs);

        foreach (var (userId, present) in eligibleValidations.Where(v => v.Present))
        {
            var participant = match.Participants.First(p => p.UserId == userId);
            if (participant.PresenceRecorded) continue;

            var user = await userRepo.GetByIdAsync(userId, ct);
            if (user is not null)
            {
                user.RecordMatchPresence();
                participant.MarkPresenceRecorded();
            }
        }

        await matchRepo.SaveChangesAsync(ct);
    }
}

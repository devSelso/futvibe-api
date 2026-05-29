using Futvibe.Domain.Entities;
using Futvibe.Domain.Enums;
using Futvibe.Domain.Exceptions;
using Futvibe.Domain.Interfaces.Repositories;
using MediatR;

namespace Futvibe.Application.Matches.Commands.UpdateParticipantStatus;

public class UpdateParticipantStatusCommandHandler(
    IMatchRepository matchRepo,
    IUserRepository userRepo,
    IMatchActivityRepository activityRepo) : IRequestHandler<UpdateParticipantStatusCommand>
{
    public async Task Handle(UpdateParticipantStatusCommand request, CancellationToken ct)
    {
        var match = await matchRepo.GetByIdWithParticipantsAsync(request.MatchId, ct)
            ?? throw new NotFoundException($"Partida {request.MatchId} não encontrada.");

        match.UpdateParticipantStatus(request.RequestingUserId, request.TargetUserId, request.NewStatus);

        var matchAlreadyHappened = match.Date <= DateOnly.FromDateTime(DateTime.UtcNow);
        if (request.NewStatus == ParticipantStatus.Confirmed && matchAlreadyHappened)
        {
            var participant = match.Participants.First(p => p.UserId == request.TargetUserId);
            if (!participant.PresenceRecorded)
            {
                var user = await userRepo.GetByIdAsync(request.TargetUserId, ct);
                if (user is not null)
                {
                    user.RecordMatchPresence();
                    participant.MarkPresenceRecorded();
                }
            }
        }

        var action = request.NewStatus == ParticipantStatus.Confirmed
            ? MatchActivityAction.Accepted
            : MatchActivityAction.Rejected;

        if (request.NewStatus is ParticipantStatus.Confirmed or ParticipantStatus.Rejected)
        {
            var log = MatchActivityLog.Create(request.MatchId, request.TargetUserId, action);
            await activityRepo.AddAsync(log, ct);
        }

        await matchRepo.SaveChangesAsync(ct);
    }
}

using Futvibe.Domain.Entities;
using Futvibe.Domain.Enums;
using Futvibe.Domain.Exceptions;
using Futvibe.Domain.Interfaces.Repositories;
using MediatR;

namespace Futvibe.Application.Ratings.Commands.SubmitRating;

public class SubmitRatingCommandHandler(
    IMatchRepository matchRepo,
    IRatingRepository ratingRepo) : IRequestHandler<SubmitRatingCommand>
{
    public async Task Handle(SubmitRatingCommand request, CancellationToken ct)
    {
        var match = await matchRepo.GetByIdWithParticipantsAsync(request.MatchId, ct)
            ?? throw new NotFoundException($"Partida {request.MatchId} não encontrada.");

        if (match.Status != MatchStatus.Closed)
            throw new BusinessException("Avaliações só podem ser feitas após o encerramento da partida.");

        var raterParticipant = match.Participants.FirstOrDefault(p => p.UserId == request.RaterId);
        if (raterParticipant is null ||
            raterParticipant.Status is not (ParticipantStatus.Confirmed or ParticipantStatus.Host))
            throw new ForbiddenException("Você não participou efetivamente desta partida.");

        var ratedParticipant = match.Participants.FirstOrDefault(p => p.UserId == request.RatedId);
        if (ratedParticipant is null)
            throw new BusinessException("O jogador avaliado não participou desta partida.");

        var alreadyRated = await ratingRepo.ExistsAsync(request.RaterId, request.RatedId, request.MatchId, ct);
        if (alreadyRated)
            throw new BusinessException("Você já avaliou este jogador nesta partida.");

        var rating = Rating.Create(request.RaterId, request.RatedId, request.MatchId, request.Score);

        await ratingRepo.AddAsync(rating, ct);
        await ratingRepo.SaveChangesAsync(ct);
    }
}

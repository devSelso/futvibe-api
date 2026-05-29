using Futvibe.Application.Common.DTOs;
using Futvibe.Application.Common.Mappers;
using Futvibe.Domain.Exceptions;
using Futvibe.Domain.Interfaces.Repositories;
using MediatR;

namespace Futvibe.Application.Matches.Queries.GetMatchById;

public class GetMatchByIdQueryHandler(IMatchRepository matchRepo)
    : IRequestHandler<GetMatchByIdQuery, MatchDto>
{
    public async Task<MatchDto> Handle(GetMatchByIdQuery request, CancellationToken ct)
    {
        var match = await matchRepo.GetByIdWithParticipantsAsync(request.Id, ct)
            ?? throw new NotFoundException($"Partida {request.Id} não encontrada.");

        if (match.TryAdvanceStatus())
            await matchRepo.SaveChangesAsync(ct);

        return MatchMapper.ToDto(match);
    }
}

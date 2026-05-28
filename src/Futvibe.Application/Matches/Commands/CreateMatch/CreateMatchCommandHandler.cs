using Futvibe.Application.Common.DTOs;
using Futvibe.Application.Common.Mappers;
using Futvibe.Domain.Entities;
using Futvibe.Domain.Interfaces.Repositories;
using MediatR;

namespace Futvibe.Application.Matches.Commands.CreateMatch;

public class CreateMatchCommandHandler(IMatchRepository matchRepo)
    : IRequestHandler<CreateMatchCommand, MatchDto>
{
    public async Task<MatchDto> Handle(CreateMatchCommand request, CancellationToken ct)
    {
        var match = Match.Create(
            request.Title,
            request.Location,
            request.Date,
            request.Time,
            request.Level,
            request.PricePerPlayer,
            request.MaxPlayers,
            request.Visibility,
            request.HostId);

        await matchRepo.AddAsync(match, ct);
        await matchRepo.SaveChangesAsync(ct);

        return MatchMapper.ToDto(match);
    }
}

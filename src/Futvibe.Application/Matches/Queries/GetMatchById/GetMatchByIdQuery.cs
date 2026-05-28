using Futvibe.Application.Common.DTOs;
using MediatR;

namespace Futvibe.Application.Matches.Queries.GetMatchById;

public record GetMatchByIdQuery(Guid Id) : IRequest<MatchDto>;

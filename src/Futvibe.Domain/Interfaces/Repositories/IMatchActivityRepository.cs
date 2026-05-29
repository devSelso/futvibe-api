using Futvibe.Domain.Entities;

namespace Futvibe.Domain.Interfaces.Repositories;

public interface IMatchActivityRepository
{
    Task AddAsync(MatchActivityLog log, CancellationToken ct = default);
    Task<IReadOnlyList<MatchActivityLog>> GetByMatchIdAsync(Guid matchId, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}

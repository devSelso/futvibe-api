using Futvibe.Domain.Entities;

namespace Futvibe.Domain.Interfaces.Repositories;

public interface IRatingRepository
{
    Task<bool> ExistsAsync(Guid raterId, Guid ratedId, Guid matchId, CancellationToken ct = default);
    Task<double?> GetAverageScoreAsync(Guid userId, CancellationToken ct = default);
    Task AddAsync(Rating rating, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}

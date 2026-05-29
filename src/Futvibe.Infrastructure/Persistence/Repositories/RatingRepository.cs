using Futvibe.Domain.Entities;
using Futvibe.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Futvibe.Infrastructure.Persistence.Repositories;

public class RatingRepository(FutvibeDbContext db) : IRatingRepository
{
    public Task<bool> ExistsAsync(Guid raterId, Guid ratedId, Guid matchId, CancellationToken ct = default)
        => db.Ratings.AnyAsync(r => r.RaterId == raterId && r.RatedId == ratedId && r.MatchId == matchId, ct);

    public async Task<double?> GetAverageScoreAsync(Guid userId, CancellationToken ct = default)
    {
        var ratings = await db.Ratings
            .Where(r => r.RatedId == userId)
            .Select(r => (int?)r.Score)
            .ToListAsync(ct);

        return ratings.Count == 0 ? null : ratings.Average();
    }

    public async Task AddAsync(Rating rating, CancellationToken ct = default)
        => await db.Ratings.AddAsync(rating, ct);

    public Task SaveChangesAsync(CancellationToken ct = default)
        => db.SaveChangesAsync(ct);
}

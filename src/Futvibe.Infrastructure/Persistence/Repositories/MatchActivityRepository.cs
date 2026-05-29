using Futvibe.Domain.Entities;
using Futvibe.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Futvibe.Infrastructure.Persistence.Repositories;

public class MatchActivityRepository(FutvibeDbContext db) : IMatchActivityRepository
{
    public async Task AddAsync(MatchActivityLog log, CancellationToken ct = default)
        => await db.MatchActivityLogs.AddAsync(log, ct);

    public async Task<IReadOnlyList<MatchActivityLog>> GetByMatchIdAsync(Guid matchId, CancellationToken ct = default)
        => await db.MatchActivityLogs
            .Include(l => l.User)
            .Where(l => l.MatchId == matchId)
            .OrderByDescending(l => l.CreatedAt)
            .ToListAsync(ct);

    public Task SaveChangesAsync(CancellationToken ct = default)
        => db.SaveChangesAsync(ct);
}

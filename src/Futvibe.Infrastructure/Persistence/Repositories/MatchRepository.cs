using Futvibe.Domain.Entities;
using Futvibe.Domain.Enums;
using Futvibe.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Futvibe.Infrastructure.Persistence.Repositories;

public class MatchRepository(FutvibeDbContext db) : IMatchRepository
{
    public Task<Match?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => db.Matches.FirstOrDefaultAsync(m => m.Id == id, ct);

    public Task<Match?> GetByIdWithParticipantsAsync(Guid id, CancellationToken ct = default)
        => db.Matches
            .Include(m => m.Participants)
                .ThenInclude(p => p.User)
            .FirstOrDefaultAsync(m => m.Id == id, ct);

    public async Task<IReadOnlyList<Match>> GetAllAsync(
        MatchLevel? level, bool? paid, int page, int limit, CancellationToken ct = default)
    {
        var query = db.Matches
            .Include(m => m.Participants)
            .AsQueryable();

        if (level.HasValue)
            query = query.Where(m => m.Level == level.Value);

        if (paid.HasValue)
            query = paid.Value
                ? query.Where(m => m.PricePerPlayer > 0)
                : query.Where(m => m.PricePerPlayer == 0);

        return await query
            .OrderBy(m => m.Date).ThenBy(m => m.Time)
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<Match>> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
        => await db.Matches
            .Include(m => m.Participants)
            .Where(m => m.Participants.Any(p => p.UserId == userId))
            .OrderByDescending(m => m.Date)
            .ToListAsync(ct);

    public async Task AddAsync(Match match, CancellationToken ct = default)
        => await db.Matches.AddAsync(match, ct);

    public Task SaveChangesAsync(CancellationToken ct = default)
        => db.SaveChangesAsync(ct);
}

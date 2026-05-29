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
        string? location, int page, int limit, CancellationToken ct = default)
    {
        var query = db.Matches
            .Include(m => m.Participants)
            .Where(m => m.Status != Domain.Enums.MatchStatus.Cancelled)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(location))
            query = query.Where(m => EF.Functions.ILike(m.Location, $"%{location}%"));

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

    public void Delete(Match match)
        => db.Matches.Remove(match);

    public Task SaveChangesAsync(CancellationToken ct = default)
        => db.SaveChangesAsync(ct);
}

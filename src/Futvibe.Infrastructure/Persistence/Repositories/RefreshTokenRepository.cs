using Futvibe.Domain.Entities;
using Futvibe.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Futvibe.Infrastructure.Persistence.Repositories;

public class RefreshTokenRepository(FutvibeDbContext db) : IRefreshTokenRepository
{
    public Task<RefreshToken?> GetByTokenHashAsync(string tokenHash, CancellationToken ct = default)
        => db.RefreshTokens.FirstOrDefaultAsync(t => t.TokenHash == tokenHash, ct);

    public async Task AddAsync(RefreshToken token, CancellationToken ct = default)
        => await db.RefreshTokens.AddAsync(token, ct);

    public Task SaveChangesAsync(CancellationToken ct = default)
        => db.SaveChangesAsync(ct);
}

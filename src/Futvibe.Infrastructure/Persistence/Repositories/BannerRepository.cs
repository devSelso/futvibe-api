using Futvibe.Domain.Entities;
using Futvibe.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Futvibe.Infrastructure.Persistence.Repositories;

public class BannerRepository(FutvibeDbContext db) : IBannerRepository
{
    public async Task<IReadOnlyList<Banner>> GetActiveAsync(CancellationToken ct = default)
        => await db.Banners
            .Where(b => b.IsActive)
            .OrderBy(b => b.DisplayOrder)
            .ToListAsync(ct);
}

using Futvibe.Domain.Entities;

namespace Futvibe.Domain.Interfaces.Repositories;

public interface IBannerRepository
{
    Task<IReadOnlyList<Banner>> GetActiveAsync(CancellationToken ct = default);
}

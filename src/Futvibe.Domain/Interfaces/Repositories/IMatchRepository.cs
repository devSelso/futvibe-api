using Futvibe.Domain.Entities;
using Futvibe.Domain.Enums;

namespace Futvibe.Domain.Interfaces.Repositories;

public interface IMatchRepository
{
    Task<Match?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Match?> GetByIdWithParticipantsAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<Match>> GetAllAsync(string? location, int page, int limit, CancellationToken ct = default);
    Task<IReadOnlyList<Match>> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task AddAsync(Match match, CancellationToken ct = default);
    void Delete(Match match);
    Task SaveChangesAsync(CancellationToken ct = default);
}

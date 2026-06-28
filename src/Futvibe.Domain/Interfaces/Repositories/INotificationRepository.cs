using Futvibe.Domain.Entities;

namespace Futvibe.Domain.Interfaces.Repositories;

public interface INotificationRepository
{
    Task<IReadOnlyList<Notification>> GetUnreadByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task<Notification?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(Notification notification, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}

using Futvibe.Domain.Entities;
using Futvibe.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Futvibe.Infrastructure.Persistence.Repositories;

public class NotificationRepository(FutvibeDbContext db) : INotificationRepository
{
    public async Task<IReadOnlyList<Notification>> GetUnreadByUserIdAsync(Guid userId, CancellationToken ct = default)
    {
        var list = await db.Notifications
            .Where(n => n.UserId == userId && !n.IsRead)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync(ct);
        return list;
    }

    public Task<Notification?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => db.Notifications.FirstOrDefaultAsync(n => n.Id == id, ct);

    public async Task AddAsync(Notification notification, CancellationToken ct = default)
        => await db.Notifications.AddAsync(notification, ct);

    public Task SaveChangesAsync(CancellationToken ct = default)
        => db.SaveChangesAsync(ct);
}

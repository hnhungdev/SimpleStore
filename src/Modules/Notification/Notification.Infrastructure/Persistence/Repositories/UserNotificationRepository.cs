
using Microsoft.EntityFrameworkCore;
using Notification.Domain.Repositories;
using Notification.Domain.Entities;
using Notification.Infrastructure.Persistence.Configurations;

namespace Notification.Infrastructure.Persistence.Repositories;

public sealed class UserNotificationRepository : IUserNotificationRepository
{
    private readonly NotificationDbContext context;
    private readonly DbSet<UserNotificationEntity> dbSet;

    public UserNotificationRepository(NotificationDbContext context)
    {
        this.context = context;
        dbSet = context.Set<UserNotificationEntity>();
    }

    public async Task AddAsync(UserNotificationEntity notification, CancellationToken ct = default)
    {
        await dbSet.AddAsync(notification, ct);
    }

    public Task<UserNotificationEntity?> GetNotificationByIdAsync(Guid id, Guid userId, CancellationToken ct = default)
    {
        return dbSet
            .FirstOrDefaultAsync(x => x.UserId == userId && x.Id == id, ct);
    }

    public Task<List<UserNotificationEntity>> GetAllNotificationsUnreadAsync(Guid userId, CancellationToken ct = default)
    {
        return dbSet
            .Where(x => x.UserId == userId && x.IsRead == false)
            .ToListAsync(ct);
    }
}
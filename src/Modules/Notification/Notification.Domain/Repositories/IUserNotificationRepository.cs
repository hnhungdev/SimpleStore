using Notification.Domain.Entities;

namespace Notification.Domain.Repositories;

public interface IUserNotificationRepository
{
    Task AddAsync(UserNotificationEntity notification, CancellationToken ct = default);

    Task<UserNotificationEntity?> GetNotificationByIdAsync(Guid id, Guid userId, CancellationToken ct = default);

    Task<List<UserNotificationEntity>> GetAllNotificationsUnreadAsync(Guid userId, CancellationToken ct = default);
}
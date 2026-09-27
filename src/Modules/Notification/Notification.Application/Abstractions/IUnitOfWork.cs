using Notification.Domain.Repositories;

namespace Notification.Application.Abstractions;

public interface IUnitOfWork
{
    INotificationJobRepository NotificationJobs { get; }

    INotificationTemplateRepository NotificationTemplates { get; }

    IUserNotificationRepository UserNotifications { get; }

    Task<int> SaveChangesAsync(CancellationToken ct = default);
}

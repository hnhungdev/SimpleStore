using Notification.Domain.Repositories;

using Notification.Application.Abstractions;

namespace Notification.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly NotificationDbContext _context;

    public INotificationJobRepository NotificationJobs { get; }

    public INotificationTemplateRepository NotificationTemplates { get; }

    public IUserNotificationRepository UserNotifications { get; }

    public UnitOfWork(NotificationDbContext context, 
        INotificationJobRepository notificationJobRepository,
        INotificationTemplateRepository notificationTemplateRepository,
        IUserNotificationRepository userNotificationRepository)
    {
        _context = context;
        NotificationJobs = notificationJobRepository;
        NotificationTemplates = notificationTemplateRepository;
        UserNotifications = userNotificationRepository;
    }
    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => _context.SaveChangesAsync(ct);
}

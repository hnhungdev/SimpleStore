using Notification.Domain.Entities;
using Notification.Domain.Enums;

namespace Notification.Domain.Repositories;

public interface INotificationTemplateRepository
{
    Task<NotificationTemplateEntity?> GetTemplateAsync(string key, ChannelType channel, CancellationToken ct = default);
}

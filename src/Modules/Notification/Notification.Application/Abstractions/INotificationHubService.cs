using Notification.Application.Features.Notification.Dtos;


namespace Notification.Application.Abstractions;

public interface INotificationHubService
{
    Task BroadcastNotificationAsync(NotificationDto notification, CancellationToken ct = default);

    Task SendToUserAsync(Guid userId, NotificationDto notification, CancellationToken ct = default);

    Task SendToUsersAsync(List<Guid> userIds, NotificationDto notification, CancellationToken ct = default);
}

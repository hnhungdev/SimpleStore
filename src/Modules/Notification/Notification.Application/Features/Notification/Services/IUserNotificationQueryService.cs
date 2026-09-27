using Notification.Application.Features.Notification.Dtos;

namespace Notification.Application.Features.Notification.Services;

public interface IUserNotificationQueryService
{
    Task<int> GetCountNotificationUnreadAsync(Guid userId, CancellationToken ct = default);

    Task<PagedResult<NotificationDto>> GetNotificationsAsync(Guid userId, NotificationFilter filter, PaginationRequest pagination, CancellationToken ct = default);

    Task<RecentNotificationsDto> GetRecentNotificationsAsync(Guid userId, CancellationToken ct = default);
}
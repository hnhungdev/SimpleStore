using Notification.Application.Features.Notification.Dtos;
using Notification.Application.Features.Notification.Services;

namespace Notification.Application.Features.Notification.Queries;

public sealed record GetRecentNotificationsQuery(Guid UserId)
    : IQuery<RecentNotificationsDto>;

public sealed class GetRecentNotificationsQueryHandler(
    IUserNotificationQueryService queryService)
    : IQueryHandler<GetRecentNotificationsQuery, RecentNotificationsDto>
{
    public async Task<RecentNotificationsDto> Handle(GetRecentNotificationsQuery query, CancellationToken ct)
    {
        return await queryService.GetRecentNotificationsAsync(query.UserId, ct);
    }
}
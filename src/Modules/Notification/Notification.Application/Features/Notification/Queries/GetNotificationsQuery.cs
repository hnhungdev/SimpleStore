using Notification.Application.Features.Notification.Dtos;
using Notification.Application.Features.Notification.Services;

namespace Notification.Application.Features.Notification.Queries;

public sealed record GetNotificationsQuery(Guid UserId, NotificationFilter Filter, PaginationRequest Pagination) 
    : IQuery<PagedResult<NotificationDto>>;

public sealed class GetNotificationsQueryHandler(
    IUserNotificationQueryService queryService)
    : IQueryHandler<GetNotificationsQuery, PagedResult<NotificationDto>>
{
    public async Task<PagedResult<NotificationDto>> Handle(GetNotificationsQuery query, CancellationToken ct)
    {
        return await queryService.GetNotificationsAsync(query.UserId, query.Filter, query.Pagination, ct);
    }
}
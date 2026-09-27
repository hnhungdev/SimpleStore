using Notification.Application.Features.Notification.Services;

namespace Notification.Application.Features.Notification.Queries;

public sealed record GetCountNotificationUnreadQuery(Guid UserId) : IQuery<int>;

public sealed class GetCountNotificationUnreadQueryHandler(
    IUserNotificationQueryService queryService)
    : IQueryHandler<GetCountNotificationUnreadQuery, int>
{
    public async Task<int> Handle(GetCountNotificationUnreadQuery query, CancellationToken ct)
    {
        return await queryService.GetCountNotificationUnreadAsync(query.UserId, ct);
    }
}

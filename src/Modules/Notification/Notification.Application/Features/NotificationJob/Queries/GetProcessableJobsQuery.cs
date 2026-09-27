using Notification.Domain.Repositories;
using Notification.Domain.Entities;

namespace Notification.Application.Features.NotificationJobs.Queries;

public sealed record GetProcessableJobsQuery(int BatchSize) : IQuery<IReadOnlyList<NotificationJobEntity>>;

public sealed class GetProcessableJobsQueryHandler(
    INotificationJobRepository repository)
    : IQueryHandler<GetProcessableJobsQuery, IReadOnlyList<NotificationJobEntity>>
{
    public async Task<IReadOnlyList<NotificationJobEntity>> Handle(GetProcessableJobsQuery query, CancellationToken ct)
    {
        return await repository.GetProcessableJobsAsync(query.BatchSize, ct);
    }
}


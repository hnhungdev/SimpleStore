using Notification.Domain.Entities;

namespace Notification.Domain.Repositories;

public interface INotificationJobRepository
{
    void Add(NotificationJobEntity job);

    void AddRange(IEnumerable<NotificationJobEntity> jobs);

    Task<IReadOnlyList<NotificationJobEntity>> GetProcessableJobsAsync(int batchSize,
        CancellationToken ct = default);

    Task<NotificationJobEntity?> GetJobByIdAsync(Guid id, CancellationToken ct = default);
}
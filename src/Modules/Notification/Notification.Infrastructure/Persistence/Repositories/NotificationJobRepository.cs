using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Notification.Domain.Repositories;
using Notification.Domain.Entities;

namespace Notification.Infrastructure.Persistence.Repositories;

public class NotificationJobRepository : INotificationJobRepository
{
    private readonly NotificationDbContext context;

    public NotificationJobRepository(NotificationDbContext context)
    {
        this.context = context;
    }

    public void Add(NotificationJobEntity job)
    {
        context.NotificationJobs.Add(job);
    }

    public void AddRange(IEnumerable<NotificationJobEntity> jobs)
    {
        context.NotificationJobs.AddRangeAsync(jobs);
    }

    public async Task<NotificationJobEntity?> GetJobByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await context.NotificationJobs.FindAsync(new object[] { id }, ct);
    }

    public async Task<IReadOnlyList<NotificationJobEntity>> GetProcessableJobsAsync(
        int batchSize,
        CancellationToken ct = default)
    {
        var now = DateTimeOffset.UtcNow;
        const int claimTimeoutMinutes = 5;

        var sql = @"
            ;WITH cte AS (
            SELECT TOP (@batchSize) *
            FROM notification.notification_jobs
            WITH (ROWLOCK, READPAST, UPDLOCK)
            WHERE processed_at IS NULL
            AND (next_attempt_at IS NULL OR next_attempt_at <= @now)
            AND attempt_count < max_attempts
            AND (
                    claimed_at IS NULL
                    OR claimed_at < DATEADD(MINUTE, -@claimTimeout, @now)
                )
            ORDER BY created_at
        )
        UPDATE cte
        SET claimed_at = @now
        OUTPUT INSERTED.id;
        ";

        var ids = new List<Guid>();

        await using var connection = context.Database.GetDbConnection();

        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync(ct);
        }

        await using var command = connection.CreateCommand();

        command.CommandText = sql;

        command.Parameters.Add(new SqlParameter("@batchSize", batchSize));
        command.Parameters.Add(new SqlParameter("@now", now));
        command.Parameters.Add(new SqlParameter("@claimTimeout", claimTimeoutMinutes));

        using (var reader = await command.ExecuteReaderAsync(ct))
        {
            while (await reader.ReadAsync(ct))
            {
                ids.Add(reader.GetGuid(0));
            }
        } 

        if (ids.Count == 0)
        {
            return [];
        }

        return await context.Set<NotificationJobEntity>()
            .Where(x => ids.Contains(x.Id))
            .ToListAsync(ct);
    }
}
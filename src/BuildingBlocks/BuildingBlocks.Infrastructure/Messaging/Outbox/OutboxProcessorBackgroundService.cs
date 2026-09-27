using System.Collections.Concurrent;
using System.Text.Json;
using MassTransit;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Infrastructure.Messaging.Outbox;

public class OutboxProcessorBackgroundService<TDbContext>(
    IServiceScopeFactory scopeFactory,
    IBus bus,
    ILogger<OutboxProcessorBackgroundService<TDbContext>> logger)
    : BackgroundService where TDbContext : DbContext
{
    private static readonly ConcurrentDictionary<string, Type> TypeCache = new();
    private const int BatchSize = 100;
    private const int MaxParallelism = 5;

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            try
            {
                var processedCount = await ProcessBatchAsync(ct);
                if (processedCount == 0)
                {
                    await Task.Delay(TimeSpan.FromSeconds(3), ct);
                }
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested) { break; }
            catch (Exception ex)
            {
                logger.LogError(ex, "Outbox processing failed for {DbContext}", typeof(TDbContext).Name);
                await Task.Delay(TimeSpan.FromSeconds(5), ct);
            }
        }
    }

    private async Task<int> ProcessBatchAsync(CancellationToken ct)
    {
        using var scope = scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<TDbContext>();

        // 1. Claim batch dùng Raw SQL CTE + UPDLOCK 
        var entityType = db.Model.FindEntityType(typeof(OutboxMessage))!;
        var table = $"[{entityType.GetSchema() ?? "dbo"}].[{entityType.GetTableName()}]";
        var now = DateTimeOffset.UtcNow;
        var claimTimeoutMinutes = 5;

        var sql = $"""
            ;WITH cte AS (
                SELECT TOP (@batchSize) *
                FROM {table} WITH (ROWLOCK, READPAST, UPDLOCK)
                WHERE processed_at IS NULL
                AND (next_attempt_at IS NULL OR next_attempt_at <= @now)
                AND attempt_count < max_attempts
                AND (
                    claimed_at IS NULL 
                    OR claimed_at < DATEADD(MINUTE, -@claimTimeout, @now)
                )
                ORDER BY occurred_at
            )
            UPDATE cte
            SET claimed_at = @now
            OUTPUT 
                INSERTED.id,
                INSERTED.event_type,
                INSERTED.content,
                INSERTED.occurred_at,
                INSERTED.processed_at,
                INSERTED.last_error_message,
                INSERTED.claimed_at,
                INSERTED.attempt_count,
                INSERTED.max_attempts,
                INSERTED.next_attempt_at;
            """;

        var messages = await db.Set<OutboxMessage>()
            .FromSqlRaw(sql,
                new SqlParameter("@batchSize", BatchSize),
                new SqlParameter("@now", now),
                new SqlParameter("@claimTimeout", claimTimeoutMinutes))
            .ToListAsync(ct);

        if (messages.Count == 0) return 0;

        // 2. Xử lý song song với Semaphore
        using var semaphore = new SemaphoreSlim(MaxParallelism);
        var tasks = messages.Select(async msg =>
        {
            await semaphore.WaitAsync(ct);
            try
            {
                var targetType = TypeCache.GetOrAdd(msg.EventType, t => Type.GetType(t)!);
                var body = JsonSerializer.Deserialize(msg.Content, targetType)!;

                await bus.Publish(body, targetType, c => c.MessageId = msg.Id, ct);
                msg.Complete(DateTimeOffset.UtcNow);
            }
            catch (Exception ex)
            {
                msg.Fail(ex.Message, DateTimeOffset.UtcNow);
                logger.LogWarning("Outbox message {Id} attempt failed: {Msg}", msg.Id, ex.Message);
            }
            finally
            {
                semaphore.Release();
            }
        });

        await Task.WhenAll(tasks);

        await db.SaveChangesAsync(ct);
        return messages.Count;
    }
}
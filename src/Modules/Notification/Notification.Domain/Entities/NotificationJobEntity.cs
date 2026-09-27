using Notification.Domain.Enums;

namespace Notification.Domain.Entities;

public sealed class NotificationJobEntity : AggregateRoot
{
    public Guid EventId { get; private set; } = default!;

    public NotificationPayload Payload { get; private set; } = default!;

    public NotificationJobStatus Status { get; private set; }

    public NotificationJobPriority Priority { get; private set; }

    public int AttemptCount { get; private set; }

    public int MaxAttempts { get; private set; }

    public string? LastErrorMessage { get; private set; }

    public DateTimeOffset? SendAt { get; private set; }

    public DateTimeOffset? ProcessedAt { get; private set; }

    public DateTimeOffset? ClaimedAt { get; private set; }

    public DateTimeOffset? NextAttemptAt { get; private set; }

    public bool IsPermanentlyFailed => AttemptCount >= MaxAttempts;

    public static NotificationJobEntity Create(
        ChannelType channel,
        Guid userId,
        string recipient,
        string subject,
        bool isHtml,
        string body,
        NotificationJobPriority priority,
        Guid eventId,
        int maxAttempts,
        string? targetUrl = null)
    {
        var payload = NotificationPayload.Create(
            channel,
            userId,
            recipient,
            subject,
            body,
            isHtml,
            targetUrl);

        return new NotificationJobEntity
        {
            EventId = eventId,
            Payload = payload,

            Status = NotificationJobStatus.Queued,
            Priority = priority,

            AttemptCount = 0,
            MaxAttempts = maxAttempts,
        };
    }

    public void MarkSent()
    {
        Status = NotificationJobStatus.Sent;

        SendAt = DateTimeOffset.UtcNow;
        ProcessedAt = DateTimeOffset.UtcNow;
    }

    public void Fail(string errorMessage, DateTimeOffset now)
    {
        IncreaseAttemptCount();

        LastErrorMessage = errorMessage;

        if (IsPermanentlyFailed)
        {
            Status = NotificationJobStatus.Failed;

            ProcessedAt = now;
            ClaimedAt = null;
            NextAttemptAt = null;
            return;
        }

        Status = NotificationJobStatus.Retrying;

        var baseDelay = TimeSpan.FromSeconds(Math.Pow(2, AttemptCount - 1));
        var maxDelay = TimeSpan.FromMinutes(5);
        var jitter = TimeSpan.FromMilliseconds(Random.Shared.Next(0, 1000));

        var delay = TimeSpan.FromTicks(Math.Min(baseDelay.Ticks, maxDelay.Ticks)) + jitter;

        NextAttemptAt = now + delay;
        ClaimedAt = null;
    }

    public void IncreaseAttemptCount()
    {
        AttemptCount++;
    }
}
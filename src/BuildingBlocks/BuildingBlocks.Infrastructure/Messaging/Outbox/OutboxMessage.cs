namespace BuildingBlocks.Infrastructure.Messaging.Outbox;

public sealed class OutboxMessage
{
    public Guid Id { get; private set; }

    public string EventType { get; private set; } = string.Empty;

    public string Content { get; private set; } = string.Empty;

    public DateTimeOffset OccurredAt { get; private set; }

    public DateTimeOffset? ProcessedAt { get; private set; }

    public string? LastErrorMessage { get; private set; }

    public DateTimeOffset? ClaimedAt { get; private set; }

    public int AttemptCount { get; private set; }

    public int MaxAttempts { get; private set; }

    public DateTimeOffset? NextAttemptAt { get; private set; }

    public bool IsPermanentlyFailed => AttemptCount >= MaxAttempts;

    public static OutboxMessage Create(
        string eventType,
        string content,
        int maxAttempts = 3)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(eventType);
        ArgumentException.ThrowIfNullOrWhiteSpace(content);

        return new OutboxMessage
        {
            Id = Guid.NewGuid(),
            EventType = eventType,
            Content = content,
            OccurredAt= DateTimeOffset.UtcNow,
            AttemptCount = 0,
            MaxAttempts = maxAttempts
        };
    }

    public void IncreaseAttemptCount()
    {
        AttemptCount++;
    }

    public void Complete(DateTimeOffset now)
    {
         if (ClaimedAt == null)
            throw new InvalidOperationException("Message must be claimed before completing.");

        ProcessedAt = now;

        ClaimedAt = null;
        NextAttemptAt = null;
    }

    public void Fail(string errorMessage, DateTimeOffset now)
    {
        IncreaseAttemptCount();

        if (IsPermanentlyFailed)
        {
            ProcessedAt = now;
            LastErrorMessage = $"Max attempts ({MaxAttempts}) exceeded. Last error: {errorMessage}";

            ClaimedAt = null;
            NextAttemptAt = null;
            return;
        }

        // exponential backoff + jitter
        var baseDelay = TimeSpan.FromSeconds(Math.Pow(2, AttemptCount - 1));
        var maxDelay = TimeSpan.FromMinutes(5);
        var jitter = TimeSpan.FromMilliseconds(Random.Shared.Next(0, 1000));

        var delay = TimeSpan.FromTicks(Math.Min(baseDelay.Ticks, maxDelay.Ticks)) + jitter;

        NextAttemptAt = now + delay;
        LastErrorMessage = errorMessage;

        ClaimedAt = null;
    }
}
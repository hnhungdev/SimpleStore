namespace BuildingBlocks.Infrastructure.Messaging.Inbox;

public sealed class InboxMessage
{
    public Guid Id { get; private set; }

    public string EventType { get; private set; } = string.Empty;

    public string Content { get; private set; } = string.Empty;

    public DateTimeOffset ReceivedAt { get; private set; }

    public DateTimeOffset? ProcessedAt { get; private set; }

    public string? LastErrorMessage { get; private set; }

    public static InboxMessage Create(
        Guid id,
        string eventType,
        string content)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(eventType);
        ArgumentException.ThrowIfNullOrWhiteSpace(content);

        return new InboxMessage
        {
            Id = id,
            EventType = eventType,
            Content = content,
            ReceivedAt = DateTimeOffset.UtcNow
        };
    }

    public void Complete(DateTimeOffset processedAt)
    {
        ProcessedAt = processedAt;
        LastErrorMessage = null;
    }

    public void RecordError(string errorMessage)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(errorMessage);

        LastErrorMessage = errorMessage;
    }
}
namespace BuildingBlocks.Domain.Abstractions;

public abstract record DomainEvent : IDomainEvent
{
    public Guid EventId { get; init; }
        = Guid.NewGuid();

    public DateTimeOffset OccurredAt { get; init; }
        = DateTimeOffset.UtcNow;

    public string EventType
        => GetType().Name;
}
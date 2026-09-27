namespace BuildingBlocks.Domain.Abstractions;

public abstract class BaseEntity 
{
    public Guid Id { get; protected set; } = Guid.NewGuid();

    public DateTimeOffset CreatedAt { get; protected set; } = DateTimeOffset.UtcNow;
}

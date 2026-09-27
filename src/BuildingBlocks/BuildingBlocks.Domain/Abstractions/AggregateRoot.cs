namespace BuildingBlocks.Domain.Abstractions;

public abstract class AggregateRoot : AuditableEntity
{
    private readonly List<IDomainEvent> _events = [];
    
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _events.AsReadOnly();

    public void AddDomainEvent(IDomainEvent @event)
    {
        _events.Add(@event);
    }

    public void ClearDomainEvents()
    {
        _events.Clear();
    }
}

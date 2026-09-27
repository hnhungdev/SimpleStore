namespace Order.Domain.Events;

public sealed record OrderDeliveredDomainEvent(Guid OrderId) : DomainEvent;
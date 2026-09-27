namespace Order.Domain.Events;

public sealed record OrderCancelledDomainEvent(Guid OrderId, string? CancelReason) : DomainEvent;
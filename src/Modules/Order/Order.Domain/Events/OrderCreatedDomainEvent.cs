namespace Order.Domain.Events;

public sealed record OrderCreatedDomainEvent(
    Guid OrderId,
    Guid UserId,
    List<OrderItemDomainEvent> OrderItems,
    decimal TotalPrice,
    string PaymentMethod
) : DomainEvent;

public sealed record OrderItemDomainEvent(Guid ProductId, int Quantity);

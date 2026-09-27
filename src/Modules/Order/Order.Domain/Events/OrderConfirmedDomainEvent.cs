using Order.Domain.Enums;

namespace Order.Domain.Events;

public sealed record OrderConfirmedDomainEvent(Guid OrderId,
    string OrderNo,
    Guid UserId,
    string CustomerName,
    string CustomerEmail,
    decimal TotalPrice,
    PaymentMethod PaymentMethod,
    DateTimeOffset ConfirmedAt
) : DomainEvent;
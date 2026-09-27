namespace Payment.Domain.Events;

public sealed record PaymentCreatedDomainEvent(
    Guid UserId,
    Guid PaymentId,
    Guid OrderId,
    string PaymentUrl,
    DateTimeOffset? ExpiresAt) : DomainEvent;
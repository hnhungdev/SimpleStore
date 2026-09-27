namespace Payment.Domain.Events;

public sealed record PaymentCreationFailedDomainEvent(
    Guid PaymentId,
    Guid OrderId,
    string FailureReason) : DomainEvent;
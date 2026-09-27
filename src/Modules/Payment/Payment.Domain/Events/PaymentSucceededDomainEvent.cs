namespace Payment.Domain.Events;

public record PaymentSucceededDomainEvent(Guid PaymentId, Guid OrderId) : DomainEvent;
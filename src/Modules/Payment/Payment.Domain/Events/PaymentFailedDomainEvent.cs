namespace Payment.Domain.Events;

public record PaymentFailedDomainEvent(Guid PaymentId, Guid OrderId, 
  string FailureReason) : DomainEvent;
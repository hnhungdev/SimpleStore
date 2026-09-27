using Payment.Domain.Constants;
using Payment.Domain.Enums;
using Payment.Domain.Events;

namespace Payment.Domain.Entities;

public sealed class PaymentEntity : AggregateRoot
{
    public Guid OrderId { get; private set; }

    public PaymentMethod PaymentMethod { get; private set; }

    public decimal Amount { get; private set; }

    public string GatewayTransactionId { get; private set; } = string.Empty;

    public string PaymentUrl { get; private set; } = string.Empty;

    public PaymentStatus Status { get; private set; }

    public DateTimeOffset? PaidAt { get; private set; }

    public DateTimeOffset? ExpiredAt { get; private set; }

    public string? FailureReason { get; private set; }

    public static PaymentEntity Create(
        Guid orderId,
        decimal amount,
        PaymentMethod paymentMethod,
        DateTimeOffset? expiredAt = null)
    {
        if (orderId == Guid.Empty)
            throw new DomainException(PaymentErrorCode.OrderIdIsRequired);

        if (amount <= 0)
            throw new ArgumentException(PaymentErrorCode.AmountMustBeGreaterThanZero);

        var payment = new PaymentEntity
        {
            OrderId = orderId,
            Amount = amount,
            PaymentMethod = paymentMethod,
            Status = PaymentStatus.Pending,
            ExpiredAt = expiredAt,
        };

        return payment;
    }

    public void InitializeGateway(
        string gatewayTransactionId,
        string paymentUrl,
        Guid userId,
        DateTimeOffset? expiredAt)
    {
        if (!string.IsNullOrEmpty(GatewayTransactionId))
            throw new DomainException(PaymentErrorCode.GatewayAlreadyInitialized);

        GatewayTransactionId = gatewayTransactionId;
        PaymentUrl = paymentUrl;
        Status = PaymentStatus.Pending;
        ExpiredAt = expiredAt;

        AddDomainEvent(new PaymentCreatedDomainEvent(userId, Id, OrderId, paymentUrl, expiredAt));
    }

    public void MarkInitializationFailed(string failureReason)
    {
        if (Status != PaymentStatus.Pending)
            throw new DomainException(PaymentErrorCode.InvalidStatusTransition);

        Status = PaymentStatus.Failed;
        FailureReason = failureReason;

        AddDomainEvent(new PaymentCreationFailedDomainEvent(Id, OrderId, FailureReason));
    }

    public void MarkSuccess()
    {
        if (Status == PaymentStatus.Success)
            return;

        if(Status != PaymentStatus.Pending)
            throw new DomainException(PaymentErrorCode.OnlyPendingTransactionCanBeMarkedAsSuccessful);

        Status = PaymentStatus.Success;
        PaidAt = DateTimeOffset.UtcNow;

        if (PaymentMethod == PaymentMethod.Cod)
            return;

        AddDomainEvent(new PaymentSucceededDomainEvent(Id, OrderId));
    }

    public void MarkFailed(string? failureReason = null)
    {
        if (Status == PaymentStatus.Failed)
            return;
        
        if (Status != PaymentStatus.Pending)
            throw new DomainException(PaymentErrorCode.OnlyPendingTransactionCanBeMarkedAsFailed);

        Status = PaymentStatus.Failed;
        FailureReason = failureReason ?? "Unknown";

        AddDomainEvent(new PaymentFailedDomainEvent(Id, OrderId, FailureReason));
    }

    public void MarkCancelled()
    {
        if(Status == PaymentStatus.Cancelled)
            return;
            
        if (Status != PaymentStatus.Pending)
            throw new DomainException(PaymentErrorCode.OnlyPendingTransactionCanBeMarkedAsCancelled);

        Status = PaymentStatus.Cancelled;
    }

    public void MarkRefunded()
    {
        if (Status == PaymentStatus.Refunded)
            return;
            
        if (Status != PaymentStatus.Success)
            throw new DomainException(PaymentErrorCode.OnlySuccessfulTransactionCanBeRefunded);

        Status = PaymentStatus.Refunded;
    }

    public void MarkExpired()
    {
        if (Status != PaymentStatus.Pending)
            return;

        Status = PaymentStatus.Expired;
    }

    public bool IsExpired => DateTimeOffset.UtcNow > ExpiredAt;
}
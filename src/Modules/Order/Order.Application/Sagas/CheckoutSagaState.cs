using MassTransit;

namespace Order.Application.Sagas;

public class CheckoutSagaState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }

    public string CurrentState { get; set; } = string.Empty;

    public Guid OrderId { get; set; }

    public Guid UserId { get; set; }

    public decimal TotalAmount { get; set; }

    public string PaymentMethod { get; set; } = string.Empty;

    public Guid? PaymentId { get; set; }

    public string? FailureReason { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    // Business deadline:
    // Payment expires at this time.
    public DateTimeOffset? PaymentExpiresAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }

    public DateTimeOffset? CompletedAt { get; set; }

    // Technical timeout:
    // Saga waits this long for InventoryReserved response.
    public Guid? ReservationTimeoutId { get; set; }

    // Technical timeout:
    // Saga waits this long for PaymentCreation response.
    public Guid? PaymentCreationTimeoutId { get; set; }

    // Technical timeout:
    // Saga waits until PaymentExpiresAt for payment completion.
    public Guid? PaymentTimeoutId { get; set; }

    public byte[] RowVersion { get; set; } = default!;
}
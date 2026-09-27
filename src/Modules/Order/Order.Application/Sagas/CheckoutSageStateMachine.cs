using MassTransit;

namespace Order.Application.Sagas;

public sealed class CheckoutSagaStateMachine : MassTransitStateMachine<CheckoutSagaState>
{
    // ============================================================
    // States
    // ============================================================

    public State AwaitingInventory { get; private set; } = null!;
    public State AwaitingPaymentCreation { get; private set; } = null!;
    public State AwaitingPayment { get; private set; } = null!;
    public State CompensatingInventory { get; private set; } = null!;
    public State Completed { get; private set; } = null!;
    public State Failed { get; private set; } = null!;

    // ============================================================
    // Events
    // ============================================================

    public Event<OrderCreatedIntegrationEvent> OrderCreated { get; private set; } = null!;
    public Event<InventoryReservedIntegrationEvent> InventoryReserved { get; private set; } = null!;
    public Event<InventoryReservationFailedIntegrationEvent> InventoryReservationFailed { get; private set; } = null!;
    public Event<PaymentCreatedIntegrationEvent> PaymentCreated { get; private set; } = null!;
    public Event<PaymentCreationFailedIntegrationEvent> PaymentCreationFailed { get; private set; } = null!;
    public Event<PaymentSucceededIntegrationEvent> PaymentSucceeded { get; private set; } = null!;
    public Event<PaymentFailedIntegrationEvent> PaymentFailed { get; private set; } = null!;
    public Event<InventoryReleasedIntegrationEvent> InventoryReleased { get; private set; } = null!;

    // ============================================================
    // Schedules
    // ============================================================

    public Schedule<CheckoutSagaState, ReservationTimeoutExpired> ReservationTimeout { get; private set; } = null!;
    public Schedule<CheckoutSagaState, PaymentCreationTimeoutExpired> PaymentCreationTimeout { get; private set; } = null!;
    public Schedule<CheckoutSagaState, PaymentTimeoutExpired> PaymentTimeout { get; private set; } = null!;

    public CheckoutSagaStateMachine()
    {
        InstanceState(x => x.CurrentState);

        const int reservationTimeoutSeconds = 30;
        const int paymentCreationTimeoutSeconds = 30;

        // ============================================================
        // Correlation
        // ============================================================

        Event(() => OrderCreated, x =>
        {
            x.CorrelateById(ctx => ctx.Message.OrderId);
            x.SelectId(ctx => ctx.Message.OrderId);
        });

        Event(() => InventoryReserved,
            x => x.CorrelateById(ctx => ctx.Message.OrderId));

        Event(() => InventoryReservationFailed,
            x => x.CorrelateById(ctx => ctx.Message.OrderId));

        Event(() => PaymentCreated,
            x => x.CorrelateById(ctx => ctx.Message.OrderId));

        Event(() => PaymentCreationFailed,
            x => x.CorrelateById(ctx => ctx.Message.OrderId));

        Event(() => PaymentSucceeded,
            x => x.CorrelateById(ctx => ctx.Message.OrderId));

        Event(() => PaymentFailed,
            x => x.CorrelateById(ctx => ctx.Message.OrderId));

        Event(() => InventoryReleased,
            x => x.CorrelateById(ctx => ctx.Message.OrderId));

        // ============================================================
        // Schedules
        // ============================================================

        Schedule(() => ReservationTimeout, x => x.ReservationTimeoutId, s =>
        {
            s.Delay = TimeSpan.FromSeconds(reservationTimeoutSeconds);
            s.Received = r => r.CorrelateById(ctx => ctx.Message.OrderId);
        });

        Schedule(() => PaymentCreationTimeout, x => x.PaymentCreationTimeoutId, s =>
        {
            s.Delay = TimeSpan.FromSeconds(paymentCreationTimeoutSeconds);
            s.Received = r => r.CorrelateById(ctx => ctx.Message.OrderId);
        });

        Schedule(() => PaymentTimeout, x => x.PaymentTimeoutId, s =>
        {
            s.Received = r => r.CorrelateById(ctx => ctx.Message.OrderId);
        });

        // ============================================================
        // 1. ORDER CREATED
        // ============================================================

        Initially(
            When(OrderCreated)
                .Then(ctx =>
                {
                    ctx.Saga.OrderId = ctx.Message.OrderId;
                    ctx.Saga.UserId = ctx.Message.UserId;
                    ctx.Saga.TotalAmount = ctx.Message.TotalPrice;
                    ctx.Saga.PaymentMethod = ctx.Message.PaymentMethod;
                    ctx.Saga.CreatedAt = DateTimeOffset.UtcNow;
                    ctx.Saga.UpdatedAt = DateTimeOffset.UtcNow;
                })
                .Schedule(ReservationTimeout, ctx => new ReservationTimeoutExpired { OrderId = ctx.Message.OrderId })
                .Publish(ctx =>
                    new InventoryReservationRequestedIntegrationEvent
                    {
                        OrderId = ctx.Saga.OrderId,
                        OrderItems = ctx.Message.OrderItems
                            .Select(item =>
                                new InventoryReservationRequestedItem
                                {
                                    ProductId = item.ProductId,
                                    Quantity = item.Quantity
                                })
                            .ToList(),
                        PaymentMethod = ctx.Saga.PaymentMethod
                    })
                .TransitionTo(AwaitingInventory)
        );

        // ============================================================
        // 2. AWAITING INVENTORY
        // ============================================================

        During(
            AwaitingInventory,

            // --------------------------------------------------------
            // Inventory reserved successfully
            // --------------------------------------------------------

            When(InventoryReserved)
                .Unschedule(ReservationTimeout)
                .Then(ctx =>
                {
                    ctx.Saga.UpdatedAt = DateTimeOffset.UtcNow;
                })
                .Schedule(PaymentCreationTimeout, ctx => new PaymentCreationTimeoutExpired { OrderId = ctx.Saga.OrderId })
                .Publish(ctx =>
                    new PaymentCreationRequestedIntegrationEvent
                    {
                        OrderId = ctx.Saga.OrderId,
                        UserId = ctx.Saga.UserId,
                        TotalPrice = ctx.Saga.TotalAmount,
                        PaymentMethod = ctx.Saga.PaymentMethod
                    })
                .TransitionTo(AwaitingPaymentCreation),

            // --------------------------------------------------------
            // Inventory reservation failed
            // --------------------------------------------------------

            When(InventoryReservationFailed)
                .Unschedule(ReservationTimeout)
                .Then(ctx =>
                {
                    ctx.Saga.FailureReason = ctx.Message.Reason ?? "Inventory reservation failed";
                    ctx.Saga.UpdatedAt = DateTimeOffset.UtcNow;
                })
                .Publish(ctx =>
                    new OrderCancellationRequestedIntegrationEvent
                    {
                        OrderId = ctx.Saga.OrderId,
                        Reason = ctx.Message.Reason ?? "Inventory reservation failed"
                    })
                .TransitionTo(Failed),

            // --------------------------------------------------------
            // Inventory timeout
            // --------------------------------------------------------

            When(ReservationTimeout.Received)
                .Then(ctx =>
                {
                    ctx.Saga.FailureReason = "Inventory reservation timeout";
                    ctx.Saga.UpdatedAt = DateTimeOffset.UtcNow;
                })
                .Publish(ctx =>
                    new OrderCancellationRequestedIntegrationEvent
                    {
                        OrderId = ctx.Saga.OrderId,
                        Reason = "Inventory reservation timeout"
                    })
                .TransitionTo(Failed)
        );

        // ============================================================
        // 3. AWAITING PAYMENT CREATION
        // ============================================================
        During(
            AwaitingPaymentCreation,

            // ============================================================
            // Payment link created successfully
            // ============================================================

            When(PaymentCreated)
                .Unschedule(PaymentCreationTimeout)
                .Then(ctx =>
                {
                    ctx.Saga.PaymentId = ctx.Message.PaymentId;
                    ctx.Saga.PaymentExpiresAt = ctx.Message.ExpiresAt;
                    ctx.Saga.UpdatedAt = DateTimeOffset.UtcNow;
                })
                .IfElse(
                    ctx => string.Equals(
                        ctx.Saga.PaymentMethod,
                        "COD",
                        StringComparison.OrdinalIgnoreCase),

                    codBinder => codBinder
                        .Then(ctx =>
                        {
                            ctx.Saga.CompletedAt = DateTimeOffset.UtcNow;
                        })
                        .Publish(ctx => new OrderConfirmationRequestedIntegrationEvent
                        {
                            OrderId = ctx.Saga.OrderId
                        })
                        .TransitionTo(Completed),

                    onlineBinder => onlineBinder
                        .Schedule(
                            PaymentTimeout,
                            ctx => new PaymentTimeoutExpired
                            {
                                OrderId = ctx.Saga.OrderId
                            },
                            ctx =>
                            {
                                var remaining = ctx.Message.ExpiresAt!.Value - DateTimeOffset.UtcNow;

                                return remaining > TimeSpan.Zero
                                    ? remaining.Add(TimeSpan.FromSeconds(10))
                                    : TimeSpan.FromSeconds(10);
                            })
                        .TransitionTo(AwaitingPayment)
                ),

            // ============================================================
            // Payment link creation failed
            // ============================================================

            When(PaymentCreationFailed)
                .Unschedule(PaymentCreationTimeout)
                .Then(ctx =>
                {
                    ctx.Saga.FailureReason = ctx.Message.FailureReason ?? "Payment creation failed";
                    ctx.Saga.UpdatedAt = DateTimeOffset.UtcNow;
                })
                .Publish(ctx => new InventoryReleaseRequestedIntegrationEvent
                {
                    OrderId = ctx.Saga.OrderId
                })
                .TransitionTo(CompensatingInventory),

            // ============================================================
            // Payment link creation timeout
            // ============================================================

            When(PaymentCreationTimeout.Received)
                .Then(ctx =>
                {
                    ctx.Saga.FailureReason = "Payment creation timeout";
                    ctx.Saga.UpdatedAt = DateTimeOffset.UtcNow;
                })
                .Publish(ctx => new InventoryReleaseRequestedIntegrationEvent
                {
                    OrderId = ctx.Saga.OrderId
                })
                .TransitionTo(CompensatingInventory),

            Ignore(ReservationTimeout.Received)
        );

        // ============================================================
        // 4. AWAITING PAYMENT
        // ============================================================

        During(
            AwaitingPayment,

            // --------------------------------------------------------
            // Payment succeeded
            // --------------------------------------------------------

            When(PaymentSucceeded)
                .Unschedule(PaymentTimeout)
                .Then(ctx =>
                {
                    ctx.Saga.PaymentId = ctx.Message.PaymentId;
                    ctx.Saga.UpdatedAt = DateTimeOffset.UtcNow;
                    ctx.Saga.CompletedAt = DateTimeOffset.UtcNow;
                })
                .Publish(ctx =>
                    new OrderConfirmationRequestedIntegrationEvent
                    {
                        OrderId = ctx.Saga.OrderId
                    })
                .TransitionTo(Completed),

            // --------------------------------------------------------
            // Payment failed
            // --------------------------------------------------------

            When(PaymentFailed)
                .Unschedule(PaymentTimeout)
                .Then(ctx =>
                {
                    ctx.Saga.FailureReason = ctx.Message.FailureReason ?? "Payment failed";
                    ctx.Saga.UpdatedAt = DateTimeOffset.UtcNow;
                })
                .Publish(ctx =>
                    new InventoryReleaseRequestedIntegrationEvent
                    {
                        OrderId = ctx.Saga.OrderId
                    })

                .TransitionTo(CompensatingInventory),

            // --------------------------------------------------------
            // Payment timeout
            // --------------------------------------------------------

            When(PaymentTimeout.Received)
                .Then(ctx =>
                {
                    ctx.Saga.FailureReason = "Payment timeout";
                    ctx.Saga.UpdatedAt = DateTimeOffset.UtcNow;
                })
                .Publish(ctx =>
                    new InventoryReleaseRequestedIntegrationEvent
                    {
                        OrderId = ctx.Saga.OrderId
                    })
                .TransitionTo(CompensatingInventory),

            Ignore(ReservationTimeout.Received)
        );

        // ============================================================
        // 5. COMPENSATING INVENTORY
        // ============================================================

        During(
            CompensatingInventory,

            // --------------------------------------------------------
            // Inventory released successfully
            // --------------------------------------------------------

            When(InventoryReleased)
                .Then(ctx =>
                {
                    ctx.Saga.UpdatedAt = DateTimeOffset.UtcNow;
                })
                .Publish(ctx =>
                    new OrderCancellationRequestedIntegrationEvent
                    {
                        OrderId = ctx.Saga.OrderId,
                        Reason = ctx.Saga.FailureReason ?? "Payment failed"
                    })
                .TransitionTo(Failed)
        );

        // ============================================================
        // 6. FAILED
        // ============================================================

        During(
            Failed,

            When(InventoryReserved)
                .Publish(ctx =>
                    new InventoryReleaseRequestedIntegrationEvent
                    {
                        OrderId = ctx.Saga.OrderId
                    })
        );
    }
}
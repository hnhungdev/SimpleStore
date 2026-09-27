namespace Order.Application.Sagas;

public sealed record ReservationTimeoutExpired
{
    public Guid OrderId { get; init; }
}

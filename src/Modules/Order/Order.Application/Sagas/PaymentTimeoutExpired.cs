namespace Order.Application.Sagas;

public sealed record PaymentTimeoutExpired
{
    public Guid OrderId { get; init; }
}

namespace Order.Application.Sagas;

public sealed record PaymentCreationTimeoutExpired
{
    public Guid OrderId { get; init; }
}
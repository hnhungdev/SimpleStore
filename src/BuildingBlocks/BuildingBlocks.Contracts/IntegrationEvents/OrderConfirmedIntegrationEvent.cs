namespace BuildingBlocks.Contracts.IntegrationEvents;

public sealed record OrderConfirmedIntegrationEvent
{
    public Guid OrderId { get; init; }

    public string OrderNo { get; init; } = string.Empty;
    
    public Guid UserId { get; init; }

    public string CustomerName { get; init; } = string.Empty;

    public string CustomerEmail { get; init; } = string.Empty;

    public decimal TotalPrice { get; init; }

    public int PaymentMethod { get; init; }

    public DateTimeOffset ConfirmedAt { get; init; }

}

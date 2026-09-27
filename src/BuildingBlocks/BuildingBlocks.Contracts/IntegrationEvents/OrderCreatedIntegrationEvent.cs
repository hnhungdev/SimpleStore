namespace BuildingBlocks.Contracts.IntegrationEvents;

public sealed record OrderCreatedIntegrationEvent
{
    public Guid OrderId { get; init; }

    public Guid UserId { get; init; }

    public List<OrderItemIntegrationEvent> OrderItems { get; init; } = new();

    public decimal TotalPrice { get; init; }

    public string PaymentMethod { get; init; } = string.Empty;
}

public sealed record OrderItemIntegrationEvent
{
    public Guid ProductId { get; init; }

    public int Quantity { get; init; }
}


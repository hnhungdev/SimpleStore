namespace BuildingBlocks.Contracts.IntegrationEvents;

public record InventoryReservationRequestedIntegrationEvent
{
    public Guid OrderId { get; init; }
    
    public List<InventoryReservationRequestedItem> OrderItems { get; init; } = new();

    public string PaymentMethod { get; init; } = string.Empty;
}

public sealed record InventoryReservationRequestedItem
{
    public Guid ProductId { get; init; }

    public int Quantity { get; init; }
}
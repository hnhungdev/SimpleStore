namespace BuildingBlocks.Contracts.IntegrationEvents;

public sealed record InventoryReservationFailedIntegrationEvent
{
    public Guid OrderId { get; init; }
    
    public string Reason { get; init; } = string.Empty;
}
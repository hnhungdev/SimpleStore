namespace BuildingBlocks.Contracts.IntegrationEvents;

public record InventoryReservedIntegrationEvent
{
    public Guid OrderId { get; init; }
}
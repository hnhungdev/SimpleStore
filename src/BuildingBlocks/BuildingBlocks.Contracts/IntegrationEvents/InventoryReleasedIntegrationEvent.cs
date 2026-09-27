namespace BuildingBlocks.Contracts.IntegrationEvents;

public record InventoryReleasedIntegrationEvent
{
    public Guid OrderId { get; init; }
}
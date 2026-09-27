namespace BuildingBlocks.Contracts.IntegrationEvents;

public sealed record InventoryReleaseRequestedIntegrationEvent
{
    public Guid OrderId { get; init; }
}
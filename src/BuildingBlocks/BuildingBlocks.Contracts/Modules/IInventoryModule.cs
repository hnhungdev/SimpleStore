namespace BuildingBlocks.Contracts.Modules;

public interface IInventoryModule
{
    Task<InventoryAvailabilityResponse> GetAvailabilityAsync(
        InventoryAvailabilityRequest request, CancellationToken ct = default);
}

public sealed record InventoryAvailabilityRequest(
    IReadOnlyCollection<InventoryCheckItem> Items);

public sealed record InventoryCheckItem(
    Guid ProductId,
    int Quantity);

public sealed record InventoryAvailabilityResponse(
    IReadOnlyList<InventoryAvailabilityItem> Items);

public sealed record InventoryAvailabilityItem(
    Guid ProductId,
    int RequestedQuantity,
    int AvailableQuantity);
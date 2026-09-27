using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Persistence.Services;

public sealed class InventoryModule(
    InventoryDbContext context) : IInventoryModule
{
    public async Task<InventoryAvailabilityResponse> GetAvailabilityAsync(
        InventoryAvailabilityRequest request,
        CancellationToken ct = default)
    {
        var productIds = request.Items
            .Select(x => x.ProductId)
            .ToArray();

        var inventories = await context.InventoryItems
            .AsNoTracking()
            .Where(x => productIds.Contains(x.ProductId))
            .GroupBy(x => x.ProductId)
            .Select(g => new
            {
                ProductId = g.Key,
                AvailableQuantity = g.Sum(x => x.OnHand - x.Reserved)
            })
            .ToDictionaryAsync(
                x => x.ProductId,
                x => x.AvailableQuantity,
                ct);

        var items = request.Items
            .Select(requestItem =>
            {
                inventories.TryGetValue(
                    requestItem.ProductId,
                    out var availableQuantity);

                return new InventoryAvailabilityItem(
                    requestItem.ProductId,
                    requestItem.Quantity,
                    availableQuantity);
            })
            .ToList();

        return new InventoryAvailabilityResponse(items);
    }
}
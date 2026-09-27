using Inventory.Domain.Entities;

namespace Inventory.Domain.Repositories;

public interface IInventoryItemRepository : IRepository<InventoryItemEntity>
{
    Task<List<InventoryItemEntity>> GetByIdsAsync(List<Guid> ids, CancellationToken ct = default);

    Task<InventoryItemEntity?> GetByProductIdAsync(Guid productId, CancellationToken ct = default);

    Task<List<InventoryItemEntity>> GetByProductIdsAsync(List<Guid> productIds, CancellationToken ct = default);
}

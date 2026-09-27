using Inventory.Domain.Entities;

namespace Inventory.Domain.Repositories;

public interface IInventoryTransactionRepository : IRepository<InventoryTransactionEntity>
{
    Task<List<InventoryTransactionEntity>> GetByInventoryItemIdAsync(
        Guid inventoryItemId, CancellationToken ct = default);
}

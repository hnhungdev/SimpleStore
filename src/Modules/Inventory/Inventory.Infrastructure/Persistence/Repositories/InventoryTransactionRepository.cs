using Inventory.Domain.Entities;
using Inventory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Persistence.Repositories;

public class InventoryTransactionRepository(InventoryDbContext context) : Repository<InventoryTransactionEntity>(context), IInventoryTransactionRepository
{
    public Task<List<InventoryTransactionEntity>> GetByInventoryItemIdAsync(Guid inventoryItemId, CancellationToken ct = default)
    {
        return _dbSet
            .Where(x => x.InventoryItemId == inventoryItemId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(ct);
    }
}

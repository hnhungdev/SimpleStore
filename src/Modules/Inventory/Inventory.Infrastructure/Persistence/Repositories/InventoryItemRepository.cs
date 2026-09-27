using Inventory.Domain.Entities;
using Inventory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Persistence.Repositories;

public class InventoryItemRepository(InventoryDbContext context)
    : Repository<InventoryItemEntity>(context), IInventoryItemRepository
{
    public Task<List<InventoryItemEntity>> GetByIdsAsync(List<Guid> ids, CancellationToken ct = default)
    {
        return _dbSet
            .Where(i => ids.Contains(i.Id))
            .ToListAsync(ct);
    }

    public async Task<InventoryItemEntity?> GetByProductIdAsync(Guid productId, CancellationToken ct = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(i => i.ProductId == productId, ct);
    }

    public async Task<List<InventoryItemEntity>> GetByProductIdsAsync(List<Guid> productIds, CancellationToken ct = default)
    {
        return await _dbSet
            .Where(i => productIds.Contains(i.ProductId))
            .ToListAsync(ct);
    }
}

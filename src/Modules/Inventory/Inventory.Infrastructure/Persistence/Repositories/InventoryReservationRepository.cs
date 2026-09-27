using Inventory.Domain.Entities;
using Inventory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Persistence.Repositories;

public class InventoryReservationRepository(InventoryDbContext context) : Repository<InventoryReservationEntity>(context), IInventoryReservationRepository
{
    public async Task<bool> ExistsByOrderIdAsync(Guid orderId, CancellationToken ct = default)
    {
        return await _dbSet
            .AnyAsync(x => x.OrderId == orderId, ct);
    }
    
    public async Task<List<InventoryReservationEntity>> GetByOrderIdAsync(Guid orderId, CancellationToken ct = default)
    {
        return await _dbSet
           .Where(x => x.OrderId == orderId)
           .ToListAsync(ct);
    }
    
    public async Task<List<InventoryReservationEntity>> GetAllAsync(CancellationToken ct = default)
    {
        return await _dbSet
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(ct);
    }
}

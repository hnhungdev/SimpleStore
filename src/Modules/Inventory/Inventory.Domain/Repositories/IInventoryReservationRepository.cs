using Inventory.Domain.Entities;

namespace Inventory.Domain.Repositories;

public interface IInventoryReservationRepository: IRepository<InventoryReservationEntity>
{
    Task<bool> ExistsByOrderIdAsync(Guid orderId, CancellationToken ct = default);
    
    Task<List<InventoryReservationEntity>> GetByOrderIdAsync(Guid orderId, CancellationToken ct = default);

    Task<List<InventoryReservationEntity>> GetAllAsync(CancellationToken ct = default);
}

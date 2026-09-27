using Inventory.Domain.Repositories;

namespace Inventory.Application.Abstractions;

public interface IUnitOfWork
{
    IInventoryItemRepository InventoryItems { get; }

    IInventoryReservationRepository InventoryReservations { get; }

    IInventoryTransactionRepository InventoryTransactions { get; }
    
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
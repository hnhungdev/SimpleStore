using Inventory.Domain.Repositories;
using Inventory.Application.Abstractions;

namespace Inventory.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly InventoryDbContext _context;

    public IInventoryItemRepository InventoryItems { get; }

    public IInventoryReservationRepository InventoryReservations { get; }

    public IInventoryTransactionRepository InventoryTransactions { get; }

    public UnitOfWork(
        InventoryDbContext context,
        IInventoryItemRepository inventoryItems,
        IInventoryReservationRepository inventoryReservations,
        IInventoryTransactionRepository inventoryTransactions)
    {
        _context = context;
        InventoryItems = inventoryItems;
        InventoryReservations = inventoryReservations;
        InventoryTransactions = inventoryTransactions;
    }

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => _context.SaveChangesAsync(ct);
}
using Microsoft.EntityFrameworkCore;
using Inventory.Application.Features.InventoryItem.Services;
using Inventory.Application.Features.InventoryItem.Dtos;
using Inventory.Application.Features.InventoryTransaction.Dtos;
using Inventory.Application.Features.InventoryReservation.Dtos;

namespace Inventory.Infrastructure.Persistence.Services;

public sealed class InventoryQueryService(InventoryDbContext context) : IInventoryQueryService
{
    public Task<PagedResult<InventoryItemDto>> GetInventoryItemsAsync(
        InventoryItemFilter filter,
        PaginationRequest pagination,
        CancellationToken ct = default)
    {
        var query = context.InventoryItems
            .AsNoTracking();

        if (!string.IsNullOrEmpty(filter.ProductName))
        {
            query = query.Where(x => x.ProductName.Contains(filter.ProductName));
        }

        var result = query
            .OrderByDescending(i => i.CreatedAt)
            .ToPagedResultAsync(
                i => new InventoryItemDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    OnHand = i.OnHand,
                    Reserved = i.Reserved,
                }, pagination, ct);

        return result;
    }

    public async Task<PagedResult<InventoryTransactionDto>> GetInventoryTransactionsAsync(
        Guid inventoryItemId,
        PaginationRequest pagination,
        CancellationToken ct = default)
    {
        var query = context.InventoryTransactions
            .AsNoTracking()
            .Where(x => x.InventoryItemId == inventoryItemId);

        var result = await query
            .OrderByDescending(i => i.CreatedAt)
            .ToPagedResultAsync(
                i => new InventoryTransactionDto
                {
                    Id = i.Id,
                    InventoryItemId = i.InventoryItemId,
                    TransactionType = i.TransactionType,
                    OnHandChanged = i.OnHandChanged,
                    ReservedChanged = i.ReservedChanged,
                    OnHandAfter = i.OnHandAfter,
                    ReservedAfter = i.ReservedAfter,
                    ReferenceId = i.ReferenceId,
                    CreatedAt = i.CreatedAt,
                }, pagination, ct);

        return result;
    }

    public async Task<PagedResult<InventoryReservationDto>> GetInventoryReservationsAsync(
        InventoryReservationFilter filter,
        PaginationRequest pagination,
        CancellationToken ct = default)
    {
        var query = context.InventoryReservations
            .AsNoTracking();

        if (filter.OrderId.HasValue)
        {
            query = query.Where(x => x.OrderId == filter.OrderId.Value);
        }

        if (filter.InventoryItemId.HasValue)
        {
            query = query.Where(x => x.InventoryItemId == filter.InventoryItemId.Value);
        }

        var result = await query
            .OrderByDescending(i => i.CreatedAt)
            .ToPagedResultAsync(
                i => new InventoryReservationDto
                {
                    Id = i.Id,
                    InventoryItemId = i.InventoryItemId,
                    OrderId = i.OrderId,
                    Quantity = i.Quantity,
                    Status = i.Status,
                    CreatedAt = i.CreatedAt,
                }, pagination, ct);

        return result;
    }
}
using Inventory.Application.Features.InventoryItem.Dtos;
using Inventory.Application.Features.InventoryReservation.Dtos;
using Inventory.Application.Features.InventoryTransaction.Dtos;

namespace Inventory.Application.Features.InventoryItem.Services;

public interface IInventoryQueryService
{
    Task<PagedResult<InventoryItemDto>> GetInventoryItemsAsync(
        InventoryItemFilter filter,
        PaginationRequest pagination,
        CancellationToken ct = default);

    Task<PagedResult<InventoryTransactionDto>> GetInventoryTransactionsAsync(
        Guid inventoryItemId,
        PaginationRequest pagination,
        CancellationToken ct = default);

    Task<PagedResult<InventoryReservationDto>> GetInventoryReservationsAsync(
        InventoryReservationFilter filter,
        PaginationRequest pagination,
        CancellationToken ct = default);
}
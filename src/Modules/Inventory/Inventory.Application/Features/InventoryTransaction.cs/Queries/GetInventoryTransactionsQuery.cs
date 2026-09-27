using Inventory.Application.Features.InventoryItem.Services;
using Inventory.Application.Features.InventoryTransaction.Dtos;

namespace Inventory.Application.Features.InventoryTransaction.Queries;

public sealed record GetInventoryTransactionsQuery(
    Guid InventoryItemId,
    PaginationRequest Pagination) : IQuery<PagedResult<InventoryTransactionDto>>;

public sealed class GetInventoryItemsQueryHandler(IInventoryQueryService inventoryQueryService)
    : IQueryHandler<GetInventoryTransactionsQuery, PagedResult<InventoryTransactionDto>>
{
    public async Task<PagedResult<InventoryTransactionDto>> Handle(GetInventoryTransactionsQuery query, CancellationToken ct = default)
    {
        return await inventoryQueryService.GetInventoryTransactionsAsync(query.InventoryItemId, query.Pagination, ct);
    }
}
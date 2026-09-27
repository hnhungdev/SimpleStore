using Inventory.Application.Features.InventoryItem.Services;
using Inventory.Application.Features.InventoryItem.Dtos;

namespace Inventory.Application.Features.InventoryItem.Queries;

public sealed record GetInventoryItemsQuery(
    InventoryItemFilter Filter,
    PaginationRequest Pagination) : IQuery<PagedResult<InventoryItemDto>>;

public sealed class GetInventoryItemsQueryHandler(IInventoryQueryService inventoryQueryService)
    : IQueryHandler<GetInventoryItemsQuery, PagedResult<InventoryItemDto>>
{
    public async Task<PagedResult<InventoryItemDto>> Handle(GetInventoryItemsQuery query, CancellationToken ct)
    {
        return await inventoryQueryService.GetInventoryItemsAsync(query.Filter, query.Pagination, ct);
    }
}
using Inventory.Application.Features.InventoryItem.Services;
using Inventory.Application.Features.InventoryReservation.Dtos;

namespace Inventory.Application.Features.InventoryReservation.Queries;

public sealed record GetInventoryReservationQuery(
    InventoryReservationFilter Filter,
    PaginationRequest Pagination) : IQuery<PagedResult<InventoryReservationDto>>;

public sealed class GetInventoryReservationQueryHandler(IInventoryQueryService inventoryQueryService)
    : IQueryHandler<GetInventoryReservationQuery, PagedResult<InventoryReservationDto>>
{
    public async Task<PagedResult<InventoryReservationDto>> Handle(GetInventoryReservationQuery query, CancellationToken ct)
    {
        return await inventoryQueryService.GetInventoryReservationsAsync(query.Filter, query.Pagination, ct);
    }
}

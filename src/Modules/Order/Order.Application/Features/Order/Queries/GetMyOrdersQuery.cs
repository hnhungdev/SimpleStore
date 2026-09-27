using Order.Application.Features.Order.Dtos;

namespace Order.Application.Features.Order.Queries;

public sealed record GetMyOrdersQuery(
    Guid UserId,
    GetMyOrdersFilter Filter,
    PaginationRequest Pagination) : IQuery<PagedResult<OrderSummaryDto>>;

public sealed class GetMyOrdersQueryHandler(IOrderQueryService orderQueryService)
    : IQueryHandler<GetMyOrdersQuery, PagedResult<OrderSummaryDto>>
{
    public async Task<PagedResult<OrderSummaryDto>> Handle(GetMyOrdersQuery query, CancellationToken ct)
    {   
        return await orderQueryService.GetMyOrdersAsync(query.UserId, query.Filter, query.Pagination, ct);
    }
}
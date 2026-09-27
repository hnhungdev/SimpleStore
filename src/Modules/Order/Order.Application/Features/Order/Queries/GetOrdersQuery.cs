using Order.Application.Features.Order.Dtos;

namespace Order.Application.Features.Order.Queries;

public sealed record GetOrdersQuery(
    GetOrdersFilter Filter,
    PaginationRequest Pagination) : IQuery<PagedResult<OrderSummaryDto>>;

public sealed class GetOrdersQueryHandler(IOrderQueryService orderQueryService)
    : IQueryHandler<GetOrdersQuery, PagedResult<OrderSummaryDto>>
{
    public async Task<PagedResult<OrderSummaryDto>> Handle(GetOrdersQuery query, CancellationToken ct)
    {
        return await orderQueryService.GetOrdersAsync(query.Filter, query.Pagination, ct);
    }
}
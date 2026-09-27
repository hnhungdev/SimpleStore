using Order.Application.Features.Order.Dtos;

namespace Order.Application.Features.Order.Queries;

public sealed record GetAllOrdersQuery(GetOrdersFilter Filter) : IQuery<List<OrderDto>>;

public sealed class GetAllOrdersQueryHandler(IOrderQueryService orderQueryService)
    : IQueryHandler<GetAllOrdersQuery, List<OrderDto>>
{
    public async Task<List<OrderDto>> Handle(GetAllOrdersQuery query, CancellationToken ct)
    {
        var orders = await orderQueryService.GetAllOrdersAsync(query.Filter, ct);

        return orders;
    }
}
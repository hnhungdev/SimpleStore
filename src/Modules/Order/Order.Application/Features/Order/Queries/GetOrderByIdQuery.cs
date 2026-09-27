using Order.Application.Features.Order.Dtos;
using Order.Domain.Constants;

namespace Order.Application.Features.Order.Queries;

public sealed record GetOrderByIdQuery(Guid OrderId) : IQuery<OrderDto>;

public sealed class GetOrderByIdQueryHandler(IOrderQueryService orderQueryService)
    : IQueryHandler<GetOrderByIdQuery, OrderDto>
{
    public async Task<OrderDto> Handle(GetOrderByIdQuery query, CancellationToken ct)
    {
        var order = await orderQueryService.GetOrderByIdAsync(query.OrderId)
            ?? throw new NotFoundException(OrderErrorCode.OrderNotFound);

        return order;
    }
}
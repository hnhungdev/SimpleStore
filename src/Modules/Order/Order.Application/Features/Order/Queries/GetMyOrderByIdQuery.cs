using Order.Application.Features.Order.Dtos;
using Order.Domain.Constants;

namespace Order.Application.Features.Order.Queries;

public sealed record GetMyOrderByIdQuery(Guid OrderId, Guid UserId) : IQuery<OrderDto>;

public sealed class GetMyOrderByIdQueryValidator : AbstractValidator<GetMyOrderByIdQuery>
{
    public GetMyOrderByIdQueryValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithMessage(OrderErrorCode.OrderIdIsRequired);

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage(OrderErrorCode.CustomerIdIsRequired);
    }
}

public sealed class GetMyOrderByIdQueryHandler(IOrderQueryService orderQueryService)
    : IQueryHandler<GetMyOrderByIdQuery, OrderDto>
{
    public async Task<OrderDto> Handle(GetMyOrderByIdQuery query, CancellationToken ct)
    {
        var order = await orderQueryService.GetMyOrderByIdAsync(query.OrderId, query.UserId, ct)
            ?? throw new NotFoundException(OrderErrorCode.OrderNotFound);

        return order;
    }
}

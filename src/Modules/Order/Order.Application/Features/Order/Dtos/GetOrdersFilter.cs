namespace Order.Application.Features.Order.Dtos;

public sealed class GetOrdersFilter : OrderFilterBase
{
    public Guid? CustomerId { get; set; }
}
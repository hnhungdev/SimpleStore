using Order.Domain.Enums;

namespace Order.Application.Features.Order.Dtos;

public abstract class OrderFilterBase
{
    public string? Keyword { get; set; }
    
    public OrderStatus? OrderStatus { get; set; }

    public DateOnly? FromDate { get; set; }

    public DateOnly? ToDate { get; set; }
}
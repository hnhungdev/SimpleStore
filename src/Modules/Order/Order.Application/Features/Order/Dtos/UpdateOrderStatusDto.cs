using Order.Domain.Enums;

namespace Order.Application.Features.Order.Dtos;

public sealed class UpdateOrderStatusDto
{
    public OrderStatus Status { get; set; }

    public string? Reason { get; set; }
}
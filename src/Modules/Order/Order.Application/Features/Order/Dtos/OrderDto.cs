using Order.Domain.Enums;

namespace Order.Application.Features.Order.Dtos;

public class OrderDto
{
    public Guid Id { get; set; }

    public string OrderNo { get; set; } = string.Empty;

    public CustomerDto Customer { get; set; } = default!;

    public List<OrderItemDto> OrderItems { get; set; } = new();

    public OrderStatus OrderStatus { get; set; }

    public PaymentMethod PaymentMethod { get; set; }

    public decimal TotalPrice { get; set;}

    public string? Notes { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
}

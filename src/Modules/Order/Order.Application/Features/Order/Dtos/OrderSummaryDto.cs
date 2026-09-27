using Order.Domain.Enums;

public class OrderSummaryDto
{
    public Guid Id { get; set; }
    
    public string OrderNo { get; set; } = string.Empty;

    public string CustomerName { get; set; } = string.Empty;

    public decimal TotalPrice { get; set; }

    public OrderStatus OrderStatus { get; set; }

    public PaymentMethod PaymentMethod { get; set; }

    public int TotalItems { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
}
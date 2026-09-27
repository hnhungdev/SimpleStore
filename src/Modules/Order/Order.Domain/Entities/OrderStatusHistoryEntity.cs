using Order.Domain.Enums;

namespace Order.Domain.Entities;

public sealed class OrderStatusHistoryEntity : AuditableEntity
{
    public Guid OrderId { get; private set; }

    public OrderStatus FromStatus { get; private set; }

    public OrderStatus ToStatus { get; private set; }

    public string? Reason { get; private set; }

    private OrderStatusHistoryEntity() { }

    public static OrderStatusHistoryEntity Create(Guid orderId, 
        OrderStatus fromStatus, OrderStatus toStatus, string? reason)
    {
        return new OrderStatusHistoryEntity
        {
            OrderId = orderId,
            FromStatus = fromStatus,
            ToStatus = toStatus,
            Reason = reason,
        };
    }
}

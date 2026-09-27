using Order.Domain.Constants;
using Order.Domain.ValueObjects;

namespace Order.Domain.Entities;

public sealed class OrderItemEntity : BaseEntity
{
    public Guid OrderId { get; private set; }

    public Product Product { get; private set; } = default!;

    public int Quantity { get; private set; }

    public decimal LineTotal => Product.Price * Quantity;

    private OrderItemEntity() { }

    public static OrderItemEntity Create(
        Guid orderId,
        Product product,
        int quantity)
    {
        if (orderId == Guid.Empty)
            throw new DomainException(OrderErrorCode.OrderIdIsRequired);

        if (quantity <= 0)
            throw new DomainException(OrderErrorCode.QuantityMustBeGreaterThanZero);

        return new OrderItemEntity
        {
            OrderId = orderId,
            Product = product,
            Quantity = quantity
        };
    }
}
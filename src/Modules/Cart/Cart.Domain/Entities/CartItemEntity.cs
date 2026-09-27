using Cart.Domain.Constants;
using Cart.Domain.ValueObjects;

namespace Cart.Domain.Entities;

public sealed class CartItemEntity : BaseEntity
{
    public Guid CartId { get; private set; } 

    public Product Product { get; private set; } = default!;

    public int Quantity { get; private set; }

    public decimal LineTotal => Product.Price * Quantity;

    private CartItemEntity() { }

    public static CartItemEntity Create(
        Guid cartId,
        Product product,
        int quantity)
    {
        if (cartId == Guid.Empty)
            throw new DomainException(CartErrorCode.CartIdIsRequired);

        if (quantity <= 0)
            throw new DomainException(CartErrorCode.QuantityMustBeGreaterThanZero);

        return new CartItemEntity
        {
            CartId = cartId,
            Product = product,
            Quantity = quantity
        };
    }

    public void IncreaseQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException(CartErrorCode.QuantityMustBeGreaterThanZero);

        Quantity += quantity;
    }

    public void SetQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException(CartErrorCode.QuantityMustBeGreaterThanZero);

        Quantity = quantity;
    }
}
using Cart.Domain.Constants;
using Cart.Domain.ValueObjects;

namespace Cart.Domain.Entities;

public sealed class CartEntity : AggregateRoot
{
    public Guid UserId { get; private set; }

    private readonly List<CartItemEntity> _items = [];

    public IReadOnlyList<CartItemEntity> Items => _items.AsReadOnly();

    public decimal TotalPrice => _items.Sum(x => x.LineTotal);

    public static CartEntity Create(Guid userId)
    {
        if(userId == Guid.Empty)
            throw new DomainException(CartErrorCode.UserIdIsRequired);

        return new CartEntity
        {
            UserId = userId
        };
    }

    public void AddItem(Product product, int quantity = 1)
    {
        var existing = Find(product.Id);

        if (existing is not null)
        {
            existing.IncreaseQuantity(quantity);

            return;
        }

        var item = CartItemEntity.Create(Id, product, quantity);

        _items.Add(item);
    }

    public void SetQuantity(Guid productId, int quantity)
    {
        var existing = Find(productId);

        if (existing is null)
            return;

        if (quantity <= 0)
        {
            _items.Remove(existing);
            return;
        }

        existing.SetQuantity(quantity);
    }

    public void RemoveItem(Guid productId)
    {
        var existing = Find(productId);

        if (existing is null)
            return;

        _items.Remove(existing);
    }

    public void Clear() => _items.Clear();

    private CartItemEntity? Find(Guid productId)
        => _items.FirstOrDefault(x => x.Product.Id == productId);
}
using Inventory.Domain.Constants;

namespace Inventory.Domain.Entities;

public sealed class InventoryItemEntity : AggregateRoot
{
    public Guid ProductId { get; private set; }

    public string ProductName { get; private set; } = string.Empty;

    public int OnHand { get; private set; }

    public int Reserved { get; private set; }

    public int Available => OnHand - Reserved;

    public static InventoryItemEntity Create(
        Guid productId,
        string productName,
        int quantity)
    {
        if (quantity <= 0)
            throw new DomainException(InventoryErrorCode.QuantityMustBeGreaterThanZero);

        if (productId == Guid.Empty)
            throw new DomainException(InventoryErrorCode.ProductIdIsRequired);

        if (string.IsNullOrWhiteSpace(productName))
            throw new DomainException(InventoryErrorCode.ProductNameIsRequired);

        var entity = new InventoryItemEntity
        {
            ProductId = productId,
            ProductName = productName,
            OnHand = quantity,
            Reserved = 0,
        };

        return entity;
    }

    public void Increase(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException(InventoryErrorCode.QuantityMustBeGreaterThanZero);

        OnHand += quantity;
    }

    public void Decrease(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException(InventoryErrorCode.QuantityMustBeGreaterThanZero);

        if (Available < quantity)
            throw new DomainException(InventoryErrorCode.InsufficientStock);

        OnHand -= quantity;
    }

    public void Reserve(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException(InventoryErrorCode.QuantityMustBeGreaterThanZero);

        if (Available < quantity)
            throw new DomainException(InventoryErrorCode.InsufficientStock);

        Reserved += quantity;
    }

    public void Release(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException(InventoryErrorCode.QuantityMustBeGreaterThanZero);

        if (Reserved < quantity)
            throw new DomainException(InventoryErrorCode.InsufficientStock);

        Reserved -= quantity;
    }

    public void Commit(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException(InventoryErrorCode.QuantityMustBeGreaterThanZero);

        if (Reserved < quantity)
            throw new DomainException(InventoryErrorCode.InsufficientStock);

        OnHand -= quantity;
        Reserved -= quantity;
    }
}

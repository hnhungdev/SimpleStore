using Inventory.Domain.Constants;
using Inventory.Domain.Enums;

namespace Inventory.Domain.Entities;

public sealed class InventoryReservationEntity : AggregateRoot
{
    public Guid OrderId { get; private set; }

    public Guid InventoryItemId { get; private set; }

    public int Quantity { get; private set; }

    public ReservationStatus Status { get; private set; }

    public static InventoryReservationEntity Create(
        Guid orderId,
        Guid inventoryItemId,
        int quantity)
    {
        if (orderId == Guid.Empty)
            throw new DomainException(InventoryErrorCode.OrderIdIsRequired);

        if (inventoryItemId == Guid.Empty)
            throw new DomainException(InventoryErrorCode.InventoryItemIdIsRequired);

        if (quantity <= 0)
            throw new DomainException(InventoryErrorCode.QuantityMustBeGreaterThanZero);

        var entity = new InventoryReservationEntity
        {
            OrderId = orderId,
            InventoryItemId = inventoryItemId,
            Quantity = quantity,
            Status = ReservationStatus.Pending,
        };

        return entity;
    }

    public void Release()
    {
        if (Status != ReservationStatus.Pending)
            throw new DomainException(InventoryErrorCode.OnlyPendingReservationsCanBeReleased);

        Status = ReservationStatus.Released;
    }
    public void Commit()
    {
        if (Status != ReservationStatus.Pending)
            throw new DomainException(InventoryErrorCode.OnlyPendingReservationsCanBeCommitted);

        Status = ReservationStatus.Committed;
    }
}

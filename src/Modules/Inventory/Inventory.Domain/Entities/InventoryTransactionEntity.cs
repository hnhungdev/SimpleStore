using Inventory.Domain.Constants;
using Inventory.Domain.Enums;

namespace Inventory.Domain.Entities;

public sealed class InventoryTransactionEntity : BaseEntity
{
    public Guid InventoryItemId { get; private set; }

    public InventoryTransactionType TransactionType { get; private set; }

    public int OnHandChanged { get; private set; }

    public int ReservedChanged { get; private set; }

    public int OnHandAfter { get; private set; }

    public int ReservedAfter { get; private set; }

    public Guid? ReferenceId { get; private set; }

    public static InventoryTransactionEntity Create(
        Guid inventoryItemId,
        InventoryTransactionType transactionType,
        int onHandChanged,
        int reservedChanged,
        int onHandAfter,
        int reservedAfter,
        Guid? referenceId = null)
    {

        if (inventoryItemId == Guid.Empty)
            throw new DomainException(InventoryErrorCode.InventoryItemIdIsRequired);
            
        return new InventoryTransactionEntity
        {
            InventoryItemId = inventoryItemId,
            TransactionType = transactionType,
            OnHandChanged = onHandChanged,
            ReservedChanged = reservedChanged,
            OnHandAfter = onHandAfter,
            ReservedAfter = reservedAfter,
            ReferenceId = referenceId,
        };
    }
}
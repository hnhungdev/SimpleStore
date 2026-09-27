namespace Inventory.Domain.Enums;

public enum InventoryTransactionType
{
    Increase = 1,
    Decrease = 2,
    Reserve = 3,
    Release = 4,
    Commit = 5,
}
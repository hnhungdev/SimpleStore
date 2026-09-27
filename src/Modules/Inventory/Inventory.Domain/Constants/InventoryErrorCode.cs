namespace Inventory.Domain.Constants;

public sealed class InventoryErrorCode
{
    public const string BadRequest = "BAD_REQUEST";

    public const string ProductIdIsRequired = "PRODUCT_ID_IS_REQUIRED";

    public const string ProductNameIsRequired = "PRODUCT_NAME_IS_REQUIRED";

    public const string ProductNotFound = "PRODUCT_NOT_FOUND";

    public const string OrderIdIsRequired = "ORDER_ID_IS_REQUIRED";

    public const string QuantityMustBeGreaterThanZero = "QUANTITY_MUST_BE_GREATER_THAN_ZERO";

    public const string InsufficientStock = "INSUFFICIENT_STOCK";

    public const string InventoryItemIdIsRequired = "INVENTORY_ITEM_ID_IS_REQUIRED";

    public const string InventoryItemAlreadyExists = "INVENTORY_ITEM_ALREADY_EXISTS";

    public const string InventoryItemNotFound = "INVENTORY_ITEM_NOT_FOUND";

    public const string OneOrMoreInventoryItemsNotFound = "ONE_OR_MORE_INVENTORY_ITEMS_NOT_FOUND";

    public const string InventoryReservationAlreadyExists = "INVENTORY_RESERVATION_ALREADY_EXISTS";

    public const string InventoryReservationNotFound = "INVENTORY_RESERVATION_NOT_FOUND";

    public const string OnlyPendingReservationsCanBeReleased = "ONLY_PENDING_RESERVATIONS_CAN_BE_RELEASED";

    public const string OnlyPendingReservationsCanBeCommitted = "ONLY_PENDING_RESERVATIONS_CAN_BE_COMMITTED";
}

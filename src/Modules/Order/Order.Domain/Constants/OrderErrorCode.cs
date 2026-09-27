namespace Order.Domain.Constants;

public sealed class OrderErrorCode
{
    public const string BadRequest = "BAD_REQUEST";

    public const string InvalidEmailAddress = "INVALID_EMAIL_ADDRESS";

    public const string InvalidPhoneNumber = "INVALID_PHONE_NUMBER";

    public const string CustomerIdIsRequired = "CUSTOMER_ID_IS_REQUIRED";

    public const string CustomerNameIsRequired = "CUSTOMER_NAME_IS_REQUIRED";

    public const string CustomerEmailIsRequired = "CUSTOMER_EMAIL_IS_REQUIRED";

    public const string CustomerPhoneNumberIsRequired = "CUSTOMER_PHONE_NUMBER_IS_REQUIRED";

    public const string CustomerAddressIsRequired = "CUSTOMER_ADDRESS_IS_REQUIRED";

    public const string ProductIdIsRequired = "PRODUCT_ID_IS_REQUIRED";

    public const string ProductNameIsRequired = "PRODUCT_NAME_IS_REQUIRED";
    
    public const string ProductThumbnailUrlIsRequired = "PRODUCT_THUMBNAIL_URL_IS_REQUIRED";

    public const string QuantityMustBeGreaterThanZero = "QUANTITY_MUST_BE_GREATER_THAN_ZERO";

    public const string PriceMustBeGreaterThanZero = "PRICE_MUST_BE_GREATER_THAN_ZERO";

    public const string OrderItemsIsRequired = "ORDER_ITEMS_IS_REQUIRED";

    public const string OrderIdIsRequired = "ORDER_ID_IS_REQUIRED";

    public const string UserIdIsRequired = "USER_ID_IS_REQUIRED";
 
    public const string InvalidOrderStatus = "INVALID_ORDER_STATUS";

    public const string InvalidPaymentMethod = "INVALID_PAYMENT_METHOD";

    public const string CancelReasonIsRequired = "CANCEL_REASON_IS_REQUIRED";

    public const string OrderNotFound = "ORDER_NOT_FOUND";

    public const string InsufficientStock = "INSUFFICIENT_STOCK";

    public const string CartIsEmpty = "CART_IS_EMPTY";

    public const string CartNotFound = "CART_NOT_FOUND";

    public const string ProductNotFound = "PRODUCT_NOT_FOUND";

    public const string DuplicateOrderItem = "DUPLICATE_ORDER_ITEM_IS_NOT_ALLOWED";

    public const string OrderIsNotEditable = "ORDER_IS_NOT_EDITABLE";

    public const string OrderCannotBeCanceled = "ORDER_CANNOT_BE_CANCELED";

    public const string OrderCannotBeDelivered = "ORDER_CANNOT_BE_DELIVERED";

    public const string OrderCannotBeShipped = "ORDER_CANNOT_BE_SHIPPED";

    public const string OrderCannotBeConfirmed = "ORDER_CANNOT_BE_CONFIRMED";
}

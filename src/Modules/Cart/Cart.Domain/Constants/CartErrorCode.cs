namespace Cart.Domain.Constants;

public static class CartErrorCode
{
    public const string UserIdIsRequired = "USER_ID_IS_REQUIRED";

    public const string ProductIdIsRequired = "PRODUCT_ID_IS_REQUIRED";

    public const string ProductNameIsRequired = "PRODUCT_NAME_IS_REQUIRED";

    public const string ProductThumbnailUrlIsRequired = "PRODUCT_THUMBNAIL_URL_IS_REQUIRED";

    public const string PriceMustBeGreaterThanZero = "PRICE_MUST_BE_GREATER_THAN_ZERO";

    public const string ProductNotFound = "PRODUCT_NOT_FOUND";

    public const string QuantityMustBeGreaterThanZero = "QUANTITY_MUST_BE_GREATER_THAN_ZERO";

    public const string MaximumQuantityExceeded = "MAXIMUM_QUANTITY_EXCEEDED_99";

    public const string CartNotFound = "CART_NOT_FOUND";

    public const string CartIdIsRequired = "CART_ID_IS_REQUIRED";
}

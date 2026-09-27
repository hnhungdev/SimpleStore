namespace Catalog.Domain.Constants;

public static class CatalogErrorCode
{
    public const string BadRequest = "BAD_REQUEST";

    public const string ProductIdIsRequired = "PRODUCT_ID_IS_REQUIRED";

    public const string ProductNameIsRequired = "PRODUCT_NAME_IS_REQUIRED";

    public const string ThumbnailUrlIsRequired = "THUMBNAIL_URL_IS_REQUIRED";

    public const string ThumbnailUrlIsInvalid = "THUMBNAIL_URL_IS_INVALID";

    public const string ImageGalleryUrlIsInvalid = "IMAGE_GALLERY_URL_IS_INVALID";

    public const string PriceMustBeGreaterThanZero = "PRICE_MUST_BE_GREATER_THAN_ZERO";

    public const string ProductNotFound = "PRODUCT_NOT_FOUND";

    public const string StatusIsInvalid = "STATUS_IS_INVALID";

    public const string SlugIsRequired = "SLUG_IS_REQUIRED";

    public const string SlugIsInvalid = "SLUG_IS_INVALID";

    public const string SlugIsAlreadyTaken = "SLUG_IS_ALREADY_TAKEN";

    public const string CategoryIdIsRequired = "CATEGORY_ID_IS_REQUIRED";

    public const string CategoryNameIsRequired = "CATEGORY_NAME_IS_REQUIRED";

    public const string CategoryParentCycleDetected = "CATEGORY_PARENT_CYCLE_DETECTED";

    public const string CategoryCannotBeItsOwnParent = "CATEGORY_CANNOT_BE_ITS_OWN_PARENT";

    public const string CategoryNotFound = "CATEGORY_NOT_FOUND";

    public const string CategoriesNotFound = "CATEGORIES_NOT_FOUND";
}

using Cart.Domain.Constants;

namespace Cart.Domain.ValueObjects;

public sealed record Product
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string ThumbnailUrl { get; init; } = string.Empty;

    public decimal Price { get; init; }

    private Product() { }

    private Product(
        Guid id,
        string name,
        string thumbnailUrl,
        decimal price)
    {
        Id = id;
        Name = name;
        ThumbnailUrl = thumbnailUrl;
        Price = price;
    }

    public static Product Of(
        Guid id,
        string name,
        string thumbnailUrl,
        decimal price)
    {
        if (id == Guid.Empty)
            throw new DomainException(CartErrorCode.ProductIdIsRequired);

        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException(CartErrorCode.ProductNameIsRequired);

        if (string.IsNullOrWhiteSpace(thumbnailUrl))
            throw new DomainException(CartErrorCode.ProductThumbnailUrlIsRequired);

        if (price <= 0)
            throw new DomainException(CartErrorCode.PriceMustBeGreaterThanZero);

        return new Product(id, name, thumbnailUrl, price);
    }
}
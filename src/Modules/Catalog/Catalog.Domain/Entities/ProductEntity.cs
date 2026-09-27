using Catalog.Domain.Constants;
using Catalog.Domain.Enums;

namespace Catalog.Domain.Entities;

public sealed class ProductEntity : AggregateRoot
{
    public string Name { get; private set; } = string.Empty;

    public string Slug { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public decimal Price { get; private set; }

    public ProductStatus Status { get; private set; }

    public string ThumbnailUrl { get; private set; } = string.Empty;

    private readonly List<ProductCategoryEntity> _productCategories = new();

    public IReadOnlyList<ProductCategoryEntity> ProductCategories
        => _productCategories.AsReadOnly();

    private readonly List<string> _galleryImageUrls = new();

    public IReadOnlyList<string> GalleryImageUrls
        => _galleryImageUrls.AsReadOnly();

    public static ProductEntity Create(
        string name,
        string slug,
        string? description,
        decimal price,
        string thumbnailUrl,
        IEnumerable<string> galleryImageUrls,
        IEnumerable<Guid> categoryIds)
    {
        ValidateProduct(name, slug, price, thumbnailUrl);

        var product = new ProductEntity
        {
            Name = name,
            Slug = slug,
            Description = description,
            Price = price,
            Status = ProductStatus.Draft,
        };

        product.SetThumbnailUrl(thumbnailUrl);
        product.SetGalleryImageUrls(galleryImageUrls);
        product.SetCategories(categoryIds);

        return product;
    }

    public void Update(
        string name,
        string slug,
        string? description,
        decimal price,
        string thumbnailUrl,
        IEnumerable<string> galleryImageUrls,
        IEnumerable<Guid> categoryIds)
    {

        ValidateProduct(name, slug, price, thumbnailUrl);

        Name = name;
        Slug = slug;
        Description = description;
        Price = price;
        SetThumbnailUrl(thumbnailUrl);
        SetGalleryImageUrls(galleryImageUrls);
        SetCategories(categoryIds);
    }

    private void SetCategories(IEnumerable<Guid> categoryIds)
    {
        var newCategoryIds = categoryIds
            .Distinct()
            .ToHashSet();

        _productCategories.RemoveAll(x => !newCategoryIds.Contains(x.CategoryId));

        var existingCategoryIds = _productCategories
            .Select(x => x.CategoryId)
            .ToHashSet();

        foreach (var categoryId in newCategoryIds)
        {
            if (!existingCategoryIds.Contains(categoryId))
            {
                _productCategories.Add(ProductCategoryEntity.Create(Id, categoryId));
            }
        }
    }

    public void SetThumbnailUrl(string newThumbnailUrl)
    {
        if (string.IsNullOrWhiteSpace(newThumbnailUrl))
            throw new DomainException("Thumbnail is required.");

        ThumbnailUrl = newThumbnailUrl;
    }

    public void SetGalleryImageUrls(IEnumerable<string> galleryImageUrls)
    {
        var newGalleryImageUrls = galleryImageUrls
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToHashSet();

        _galleryImageUrls.RemoveAll(x => !newGalleryImageUrls.Contains(x));

        foreach (var image in newGalleryImageUrls)
        {
            if (!_galleryImageUrls.Contains(image))
            {
                _galleryImageUrls.Add(image);
            }
        }
    }

    public void ChangeStatus(ProductStatus status)
    {
        if (Status == status)
        {
            return;
        }

        Status = status;
    }

    private static void ValidateProduct(string name, string slug, decimal price, string thumbnailUrl)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException(CatalogErrorCode.ProductNameIsRequired);

        if (string.IsNullOrWhiteSpace(slug))
            throw new DomainException(CatalogErrorCode.SlugIsRequired);

        if (price <= 0)
            throw new DomainException(CatalogErrorCode.PriceMustBeGreaterThanZero);

        if (string.IsNullOrWhiteSpace(thumbnailUrl))
            throw new DomainException(CatalogErrorCode.ThumbnailUrlIsRequired);
    }
}

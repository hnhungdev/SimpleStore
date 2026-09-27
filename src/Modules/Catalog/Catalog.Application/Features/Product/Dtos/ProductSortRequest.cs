namespace Catalog.Application.Features.Product.Dtos;

public sealed class ProductSortRequest
{
    public string? SortBy { get; init; }
}

public enum ProductSortType
{
    Newest,
    PriceAsc,
    PriceDesc,
    NameAsc,
    NameDesc
}
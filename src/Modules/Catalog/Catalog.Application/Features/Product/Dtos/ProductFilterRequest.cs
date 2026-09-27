using Catalog.Domain.Enums;

namespace Catalog.Application.Features.Product.Dtos;

public sealed class ProductFilterRequest
{
    public string? Keyword { get; init; }

    public decimal? MinPrice { get; init; }

    public decimal? MaxPrice { get; init; }

    public Guid? CategoryId { get; init; }

    public ProductStatus? Status { get; init; }
}

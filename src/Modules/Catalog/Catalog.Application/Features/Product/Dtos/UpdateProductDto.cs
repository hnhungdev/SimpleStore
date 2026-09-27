namespace Catalog.Application.Features.Product.Dtos;

public class UpdateProductDto
{
    public string Name { get; init; } = string.Empty;

    public string Slug { get; init; } = string.Empty;

    public string? Sku { get; init; }

    public string? Description { get; init; }

    public decimal Price { get; init; }

    public IReadOnlyList<Guid> CategoryIds { get; init; } = [];

    public IReadOnlyList<string> Images { get; init; } = [];

    public string ThumbnailUrl { get; init; } = string.Empty;

    public Guid? BrandId { get; init; }
}
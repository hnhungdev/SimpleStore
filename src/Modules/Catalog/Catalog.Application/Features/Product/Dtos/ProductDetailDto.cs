using Catalog.Domain.Enums;

namespace Catalog.Application.Features.Product.Dtos;

public sealed record ProductDetailDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public decimal Price { get; init; }

    public string ThumbnailUrl { get; init; } = string.Empty;

    public List<string> CategoryNames { get; init; } = new();

    public string? Description { get; init; }

    public ProductStatus Status { get; init; }

    public string Slug { get; init; } = default!;

    public List<string>? Images { get; init; }

    public DateTimeOffset CreatedAt { get; init; }

    public string? CreatedBy { get; init; } 

    public DateTimeOffset? UpdatedAt { get; init; }

    public string? UpdatedBy { get; init; } 
}
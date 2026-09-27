using Catalog.Domain.Enums;

namespace Catalog.Application.Features.Product.Dtos;

public sealed record ProductSummaryDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public decimal Price { get; init; }

    public string ThumbnailUrl { get; init; } = string.Empty;

    public ProductStatus Status { get; init; }

    public string Slug { get; init; } = default!;

    public DateTimeOffset CreatedAt { get; init; }
}
using Catalog.Domain.Enums;

namespace Catalog.Application.Features.Product.Dtos;

public sealed class ChangeProductStatusDto
{
    public ProductStatus Status { get; init; }
}
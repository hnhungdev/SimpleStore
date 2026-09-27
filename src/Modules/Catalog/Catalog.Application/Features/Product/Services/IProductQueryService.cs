using Catalog.Application.Features.Product.Dtos;

namespace Catalog.Application.Features.Product.Services;

public interface IProductQueryService
{
    Task<ProductDetailDto?> GetProductDetailAsync(Guid productId, CancellationToken ct);

    Task<PagedResult<ProductSummaryDto>> GetProductsAsync(
        ProductFilterRequest filter,
        PaginationRequest pagination,
        ProductSortRequest sort,
        CancellationToken ct = default);

    Task<ProductDetailDto?> GetPublicProductDetailAsync(Guid productId, CancellationToken ct);

    Task<ProductDetailDto?> GetPublicProductBySlugAsync(string slug, CancellationToken ct);

    Task<PagedResult<ProductSummaryDto>> GetPublicProductsAsync(
        ProductFilterRequest filter,
        PaginationRequest pagination,
        ProductSortRequest sort,
        CancellationToken ct = default);
}
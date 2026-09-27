using Catalog.Application.Features.Product.Dtos;
using Catalog.Application.Features.Product.Services;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Persistence.Services;

public sealed class ProductQueryService(CatalogDbContext _context)
    : IProductQueryService
{
    public async Task<ProductDetailDto?> GetProductDetailAsync(
        Guid productId,
        CancellationToken ct = default)
    {
        var product = await _context.Products
            .AsNoTracking()
            .Where(p => p.Id == productId)
            .ToDetailDto(_context.Categories)
            .FirstOrDefaultAsync(ct);

        return product;
    }

    public async Task<PagedResult<ProductSummaryDto>> GetProductsAsync(
        ProductFilterRequest filter,
        PaginationRequest pagination,
        ProductSortRequest sort,
        CancellationToken ct = default)
    {
        return await _context.Products
            .AsNoTracking()
            .ApplyProductFilter(filter)
            .ApplyProductSort(sort)
            .ToPagedResultAsync(ProductQueryExtensions.ToSummaryDto(), pagination, ct);
    }

    public async Task<ProductDetailDto?> GetPublicProductBySlugAsync(
        string slug,
        CancellationToken ct = default)
    {
        var product = await _context.Products
            .AsNoTracking()
            .ApplyPublicAccess()
            .Where(p => p.Slug == slug)
            .ToDetailDto(_context.Categories)
            .FirstOrDefaultAsync(ct);

        return product;
    }

    public async Task<ProductDetailDto?> GetPublicProductDetailAsync(
        Guid productId,
        CancellationToken ct = default)
    {
        var product = await _context.Products
            .AsNoTracking()
            .ApplyPublicAccess()
            .Where(p => p.Id == productId)
            .ToDetailDto(_context.Categories)
            .FirstOrDefaultAsync(ct);

        return product;
    }

    public async Task<PagedResult<ProductSummaryDto>> GetPublicProductsAsync(
        ProductFilterRequest filter,
        PaginationRequest pagination,
        ProductSortRequest sort,
        CancellationToken ct = default)
    {
        return await _context.Products
            .AsNoTracking()
            .ApplyPublicAccess()
            .ApplyProductFilter(filter)
            .ApplyProductSort(sort)
            .ToPagedResultAsync(ProductQueryExtensions.ToSummaryDto(), pagination, ct);
    }
}
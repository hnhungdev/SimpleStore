using System.Linq.Expressions;
using Catalog.Application.Features.Product.Dtos;
using Catalog.Domain.Entities;
using Catalog.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Persistence.Services;

public static class ProductQueryExtensions
{
    public static Expression<Func<ProductEntity, ProductSummaryDto>> ToSummaryDto()
    {
        return p => new ProductSummaryDto
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            ThumbnailUrl = p.ThumbnailUrl,
            Status = p.Status,
            Slug = p.Slug,
            CreatedAt = p.CreatedAt
        };
    }

    public static IQueryable<ProductDetailDto> ToDetailDto(
        this IQueryable<ProductEntity> products,
        IQueryable<CategoryEntity> categories)
    {
        return products.Select(p => new ProductDetailDto
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            Description = p.Description,
            Slug = p.Slug,
            Status = p.Status,

            ThumbnailUrl = p.ThumbnailUrl,
            Images = p.GalleryImageUrls.ToList(),

            CategoryNames = categories
                .Where(c => p.ProductCategories
                    .Select(pc => pc.CategoryId)
                    .Contains(c.Id))
                .Select(c => c.Name)
                .ToList(),

            CreatedAt = p.CreatedAt,
            CreatedBy = p.CreatedBy,
            UpdatedAt = p.UpdatedAt,
            UpdatedBy = p.UpdatedBy
        });
    }

    public static IQueryable<ProductEntity> ApplyProductFilter(
        this IQueryable<ProductEntity> query,
        ProductFilterRequest filter)
    {
        if (filter is null)
            return query;

        if (!string.IsNullOrWhiteSpace(filter.Keyword))
        {
            query = query.Where(p =>
                EF.Functions.Collate(
                    p.Name,
                    "Vietnamese_CI_AI"
                ).Contains(filter.Keyword.Trim()));
        }

        if (filter.MinPrice.HasValue)
        {
            query = query.Where(p =>
                p.Price >= filter.MinPrice.Value);
        }

        if (filter.MaxPrice.HasValue)
        {
            query = query.Where(p =>
                p.Price <= filter.MaxPrice.Value);
        }

        if (filter.CategoryId.HasValue)
        {
            query = query.Where(p =>
                p.ProductCategories.Any(pc =>
                    pc.CategoryId == filter.CategoryId.Value));
        }

        if (filter.Status.HasValue)
        {
            query = query.Where(p =>
                p.Status == filter.Status.Value);
        }

        return query;
    }

    public static IQueryable<ProductEntity> ApplyProductSort(
        this IQueryable<ProductEntity> query,
        ProductSortRequest request)
    {
        var sort = Enum.TryParse<ProductSortType>(
            request.SortBy,
            ignoreCase: true,
            out var parsedSort)
                ? parsedSort
                : ProductSortType.Newest;

        return sort switch
        {
            ProductSortType.PriceAsc =>
                query.OrderBy(p => p.Price)
                     .ThenBy(p => p.Id),

            ProductSortType.PriceDesc =>
                query.OrderByDescending(p => p.Price)
                     .ThenBy(p => p.Id),

            ProductSortType.NameAsc =>
                query.OrderBy(p => p.Name)
                     .ThenBy(p => p.Id),

            ProductSortType.NameDesc =>
                query.OrderByDescending(p => p.Name)
                     .ThenBy(p => p.Id),

            _ =>
                query.OrderByDescending(p => p.CreatedAt)
                     .ThenBy(p => p.Id)
        };
    }

    public static IQueryable<ProductEntity> ApplyPublicAccess(
        this IQueryable<ProductEntity> query)
    {
        return query.Where(p =>
            p.Status == ProductStatus.Published);
    }
}

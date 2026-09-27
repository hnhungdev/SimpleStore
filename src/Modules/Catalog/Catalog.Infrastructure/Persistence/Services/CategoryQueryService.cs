using Catalog.Application.Features.Category.Dtos;
using Microsoft.EntityFrameworkCore;
using Catalog.Application.Features.Category.Services;
using System.Linq.Expressions;
using Catalog.Domain.Entities;

namespace Catalog.Infrastructure.Persistence.Services;

public sealed class CategoryQueryService : ICategoryQueryService
{
    private readonly CatalogDbContext _context;

    public CategoryQueryService(
        CatalogDbContext context)
    {
        _context = context;
    }

    public async Task<CategoryDto?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _context.Categories
            .Select(category => new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Slug = category.Slug,
                ParentId = category.ParentId,
                ParentName = category.Parent != null
                    ? category.Parent.Name
                    : null,
                Description = category.Description
            })

            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<List<CategoryDto>> GetAllAsync(CancellationToken ct)
    {
        return await _context.Categories
            .OrderByDescending(x => x.CreatedAt)
            .Select(category => new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Slug = category.Slug,
                ParentId = category.ParentId,
                ParentName = category.Parent != null
                    ? category.Parent.Name
                    : null,
                Description = category.Description
            })

            .ToListAsync(ct);
    }

    public async Task<PagedResult<CategoryDto>> GetPagedAsync(PaginationRequest pagination, CancellationToken ct)
    {
        return await
            _context.Categories
            .OrderByDescending(x => x.CreatedAt)
            .ToPagedResultAsync(
                x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Slug = x.Slug,
                    ParentId = x.ParentId,
                    ParentName = x.Parent != null
                        ? x.Parent.Name
                        : null,
                    Description = x.Description
                },
                pagination, ct);
    }

    public async Task<List<CategoryTreeDto>> GetTreeAsync(CancellationToken ct)
    {
        var categories = await _context.Categories
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .Select(x => new
            {
                x.Id,
                x.Name,
                x.ParentId
            })
            .ToListAsync(ct);

        var nodes = categories.ToDictionary(
            x => x.Id,
            x => new CategoryTreeDto
            {
                Id = x.Id,
                Name = x.Name
            });

        var roots = new List<CategoryTreeDto>();

        foreach (var x in categories)
        {
            var node = nodes[x.Id];

            if (x.ParentId is null)
            {
                roots.Add(node);
                continue;
            }

            if (nodes.TryGetValue(x.ParentId.Value, out var parent))
            {
                parent.Children.Add(node);
            }
        }

        return roots;
    }

    private static Expression<Func<CategoryEntity, CategoryDto>> ToDto()
    {
        return category => new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Slug = category.Slug,
            ParentId = category.ParentId,
            ParentName = category.Parent != null
        ? category.Parent.Name
        : null,
            Description = category.Description
        };
    }
}
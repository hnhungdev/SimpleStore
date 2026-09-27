using Catalog.Domain.Repositories;
using Catalog.Domain.Entities;
using Catalog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public class CategoryRepository : Repository<CategoryEntity>, ICategoryRepository
{
    public CategoryRepository(CatalogDbContext context)
        : base(context)
    {

    }

    public async Task MoveChildrenAsync(Guid categoryId, Guid? newParentId, CancellationToken ct = default)
    {
        var children = await _context.Categories
            .Where(x => x.ParentId == categoryId)
            .ToListAsync(ct);

        foreach (var child in children)
        {
            child.ChangeParent(newParentId);
        }
    }

    public async Task<bool> IsDescendantOfAsync(Guid descendantId, Guid ancestorId, CancellationToken ct = default)
    {
        var current = await _context.Categories
            .Where(x => x.Id == descendantId)
            .Select(x => x.ParentId)
            .FirstOrDefaultAsync(ct);

        while (current != null)
        {
            if (current == ancestorId)
                return true;

            current = await _context.Categories
                .Where(x => x.Id == current)
                .Select(x => x.ParentId)
                .FirstOrDefaultAsync(ct);
        }

        return false;
    }

    public async Task<List<Guid>> GetExistingIdsAsync(List<Guid> ids, CancellationToken ct)
    {
        return await _dbSet.Where(x => ids.Contains(x.Id))
            .Select(x => x.Id)
            .ToListAsync(ct);
    }

    public Task<bool> IsSlugTakenAsync(string slug, Guid? exceptId, CancellationToken ct)
    {
        return _dbSet.AnyAsync(x => x.Slug == slug && x.Id != exceptId, ct);
    }
}
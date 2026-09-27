using Catalog.Domain.Repositories;
using Catalog.Domain.Entities;
using Catalog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public class ProductRepository : Repository<ProductEntity>, IProductRepository
{
    public ProductRepository(CatalogDbContext context) : base(context)
    {
    }

    public async Task<ProductEntity?> GetByIdWithCategoriesAsync(Guid id, CancellationToken ct = default)
    {
        return await _dbSet
            .Include(p => p.ProductCategories)
            .FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public Task<bool> IsSlugTakenAsync(string slug, Guid? exceptId, CancellationToken ct)
    {
        return _dbSet.AnyAsync(x => x.Slug == slug && x.Id != exceptId, ct);
    }
}
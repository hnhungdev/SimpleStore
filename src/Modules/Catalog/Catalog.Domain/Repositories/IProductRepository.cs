using Catalog.Domain.Entities;

namespace Catalog.Domain.Repositories;

public interface IProductRepository : IRepository<ProductEntity>
{
    Task<ProductEntity?> GetByIdWithCategoriesAsync(Guid id, CancellationToken ct = default);

    Task<bool> IsSlugTakenAsync(string slug, Guid? exceptId, CancellationToken ct);
}
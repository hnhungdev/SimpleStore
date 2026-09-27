using Catalog.Domain.Entities;

namespace Catalog.Domain.Repositories;

public interface ICategoryRepository : IRepository<CategoryEntity>
{
    Task MoveChildrenAsync(Guid categoryId, Guid? newParentId, CancellationToken ct = default);

    Task<bool> IsDescendantOfAsync(Guid descendantId, Guid ancestorId, CancellationToken ct = default);

    Task<List<Guid>> GetExistingIdsAsync(List<Guid> ids, CancellationToken ct);

    Task<bool> IsSlugTakenAsync(string slug, Guid? exceptId, CancellationToken ct);
}
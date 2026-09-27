using Catalog.Application.Features.Category.Dtos;

namespace Catalog.Application.Features.Category.Services;

public interface ICategoryQueryService
{
    Task<CategoryDto?> GetByIdAsync(Guid id, CancellationToken ct);

    Task<List<CategoryDto>> GetAllAsync(CancellationToken ct);

    Task<PagedResult<CategoryDto>> GetPagedAsync(PaginationRequest pagination, CancellationToken ct);

    Task<List<CategoryTreeDto>> GetTreeAsync(CancellationToken ct);
}
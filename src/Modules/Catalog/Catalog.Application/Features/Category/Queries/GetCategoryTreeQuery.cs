using Catalog.Application.Features.Category.Dtos;
using Catalog.Application.Features.Category.Services;

namespace Catalog.Application.Features.Category.Queries;

public sealed record GetCategoryTreeQuery : IQuery<List<CategoryTreeDto>>;

public sealed class GetTreeCategoriesQueryHandler(ICategoryQueryService queryService)
    : IQueryHandler<GetCategoryTreeQuery, List<CategoryTreeDto>>
{
    public async Task<List<CategoryTreeDto>> Handle(GetCategoryTreeQuery query, CancellationToken ct)
    {
        return await queryService.GetTreeAsync(ct);
    }
}
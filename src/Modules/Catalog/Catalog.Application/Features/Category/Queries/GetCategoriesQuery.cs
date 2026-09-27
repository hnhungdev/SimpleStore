using Catalog.Application.Features.Category.Dtos;
using Catalog.Application.Features.Category.Services;

namespace Catalog.Application.Features.Category.Queries;

public sealed record GetCategoriesQuery(PaginationRequest Pagination) : IQuery<PagedResult<CategoryDto>>;

public sealed class GetCategoriesQueryHandler(ICategoryQueryService categoryQueries)
    : IQueryHandler<GetCategoriesQuery, PagedResult<CategoryDto>>
{
    public async Task<PagedResult<CategoryDto>> Handle(GetCategoriesQuery query, CancellationToken ct)
    {
        return await categoryQueries.GetPagedAsync(query.Pagination,ct);
    }
}
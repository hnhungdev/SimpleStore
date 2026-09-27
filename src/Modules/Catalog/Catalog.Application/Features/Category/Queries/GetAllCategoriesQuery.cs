using Catalog.Application.Features.Category.Dtos;
using Catalog.Application.Features.Category.Services;


namespace Catalog.Application.Features.Category.Queries;

public sealed record GetAllCategoriesQuery : IQuery<List<CategoryDto>>;

public sealed class GetAllCategoriesQueryHandler(ICategoryQueryService queryService)
    : IQueryHandler<GetAllCategoriesQuery, List<CategoryDto>>
{
    public async Task<List<CategoryDto>> Handle(GetAllCategoriesQuery query, CancellationToken ct)
    {
       return await queryService.GetAllAsync(ct);
    }
}
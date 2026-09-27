using Catalog.Domain.Constants;
using Catalog.Application.Features.Category.Dtos;
using Catalog.Application.Features.Category.Services;

namespace Catalog.Application.Features.Category.Queries;

public sealed record GetCategoryByIdQuery(Guid Id) : IQuery<CategoryDto>;

public sealed class GetCategoryByIdQueryHandler(ICategoryQueryService categoryQueries)
    : IQueryHandler<GetCategoryByIdQuery, CategoryDto>
{
    public async Task<CategoryDto> Handle(GetCategoryByIdQuery query, CancellationToken ct)
    {
        return await categoryQueries.GetByIdAsync(query.Id, ct)
            ?? throw new NotFoundException(CatalogErrorCode.CategoryNotFound);
    }
}
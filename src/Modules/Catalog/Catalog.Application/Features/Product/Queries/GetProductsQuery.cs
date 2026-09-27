using Catalog.Application.Features.Product.Dtos;
using Catalog.Application.Features.Product.Services;

namespace Catalog.Application.Features.Product.Queries;

public sealed record GetProductsQuery(
    ProductFilterRequest Filter,
    ProductSortRequest Sort,
    PaginationRequest Pagination) : IQuery<PagedResult<ProductSummaryDto>>;

public sealed class GetProductsQueryHandler(IProductQueryService productQueryService)
    : IQueryHandler<GetProductsQuery, PagedResult<ProductSummaryDto>>
{
    public async Task<PagedResult<ProductSummaryDto>> Handle(GetProductsQuery query, CancellationToken ct)
    {
        return await productQueryService
            .GetProductsAsync(query.Filter, query.Pagination, query.Sort, ct);
    }
}
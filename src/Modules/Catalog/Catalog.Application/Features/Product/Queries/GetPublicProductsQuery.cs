using Catalog.Application.Features.Product.Dtos;
using Catalog.Application.Features.Product.Services;

namespace Catalog.Application.Features.Product.Queries;

public sealed record GetPublicProductsQuery(
    ProductFilterRequest Filter,
    ProductSortRequest Sort,
    PaginationRequest Pagination) : IQuery<PagedResult<ProductSummaryDto>>;

public sealed class GetPublicProductsQueryHandler(IProductQueryService productQueryService)
    : IQueryHandler<GetPublicProductsQuery, PagedResult<ProductSummaryDto>>
{
    public async Task<PagedResult<ProductSummaryDto>> Handle(GetPublicProductsQuery query, CancellationToken ct)
    {
        return await productQueryService
            .GetPublicProductsAsync(query.Filter, query.Pagination, query.Sort, ct);
    }
}
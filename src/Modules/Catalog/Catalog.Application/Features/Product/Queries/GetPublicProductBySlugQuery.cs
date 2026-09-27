using Catalog.Application.Features.Product.Dtos;
using Catalog.Application.Features.Product.Services;
using Catalog.Domain.Constants;

namespace Catalog.Application.Features.Product.Queries;

public sealed record GetPublicProductBySlugQuery(string Slug) : IQuery<ProductDetailDto>;

public sealed class GetPublicProductBySlugHandler(IProductQueryService queryService)
    : IQueryHandler<GetPublicProductBySlugQuery, ProductDetailDto>
{
    public async Task<ProductDetailDto> Handle(GetPublicProductBySlugQuery query, CancellationToken ct)
    {
       return await queryService.GetPublicProductBySlugAsync(query.Slug, ct)
            ?? throw new NotFoundException(CatalogErrorCode.ProductNotFound);
    }
}
using Catalog.Application.Features.Product.Dtos;
using Catalog.Application.Features.Product.Services;
using Catalog.Domain.Constants;

namespace Catalog.Application.Features.Product.Queries;

public sealed record GetPublicProductByIdQuery(Guid ProductId) : IQuery<ProductDetailDto>;

public sealed class GetPublicProductByIdQueryHandler(IProductQueryService queryService)
    : IQueryHandler<GetPublicProductByIdQuery, ProductDetailDto>
{
    public async Task<ProductDetailDto> Handle(GetPublicProductByIdQuery query, CancellationToken ct)
    {
       return await queryService.GetPublicProductDetailAsync(query.ProductId, ct)
            ?? throw new NotFoundException(CatalogErrorCode.ProductNotFound);
    }
}
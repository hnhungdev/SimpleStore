using Catalog.Application.Features.Product.Dtos;
using Catalog.Application.Features.Product.Services;
using Catalog.Domain.Constants;

namespace Catalog.Application.Features.Product.Queries;

public sealed record GetProductByIdQuery(Guid ProductId) : IQuery<ProductDetailDto>;

public sealed class GetProductByIdQueryHandler(IProductQueryService queryService)
    : IQueryHandler<GetProductByIdQuery, ProductDetailDto>
{
    public async Task<ProductDetailDto> Handle(GetProductByIdQuery query, CancellationToken ct)
    {
       return await queryService.GetProductDetailAsync(query.ProductId, ct)
            ?? throw new NotFoundException(CatalogErrorCode.ProductNotFound);
    }
}
namespace Catalog.Domain.Entities;

public sealed class ProductCategoryEntity
{
    public Guid ProductId { get; private set; }

    public Guid CategoryId { get; private set; }

    private ProductCategoryEntity() { }
    
    public static ProductCategoryEntity Create(
        Guid productId,
        Guid categoryId)
    {
        return new ProductCategoryEntity
        {
            ProductId = productId,
            CategoryId = categoryId
        };
    }
}
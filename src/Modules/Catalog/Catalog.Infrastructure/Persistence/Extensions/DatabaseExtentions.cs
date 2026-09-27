using Catalog.Domain.Enums;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Infrastructure.Persistence.Extensions;

public static class DatabaseExtentions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();

        await SeedDataAsync(dbContext);
    }

    private static async Task SeedDataAsync(CatalogDbContext dbContext)
    {
        if (await dbContext.Products.AnyAsync())
            return;

        var categories = CategorySeedData.GetCategories();
        await dbContext.Categories.AddRangeAsync(categories);

        var products = ProductSeedData.GetAllProducts();

        foreach (var product in products)
        {
            product.ChangeStatus(ProductStatus.Published);
        }

        await dbContext.Products.AddRangeAsync(products);

        await dbContext.SaveChangesAsync();
    }
}

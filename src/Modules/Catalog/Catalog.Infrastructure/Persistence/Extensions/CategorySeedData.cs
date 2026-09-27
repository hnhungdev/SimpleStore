using Catalog.Domain.Entities;

namespace Catalog.Infrastructure.Persistence;

public static class CategorySeedData
{
    public static CategoryEntity[] GetCategories()
    {
        var electronics = CategoryEntity.Create(
        name: "Electronics",
        slug: "electronics",
        description: "Electronic devices & accessories");


        var phones = CategoryEntity.Create(
            name: "Phones",
            slug: "phones",
            description: "Smartphones & accessories",
            parentId: electronics.Id);

        var laptops = CategoryEntity.Create(
            name: "Laptops",
            slug: "laptops",
            description: "Laptops & accessories",
            parentId: electronics.Id);

        var fashion = CategoryEntity.Create(
            name: "Fashion",
            slug: "fashion",
            description: "Clothing, shoes & accessories");

        var menFashion = CategoryEntity.Create(
            name: "Men",
            slug: "men-fashion",
            description: "Men's fashion",
            parentId: fashion.Id);

        var womenFashion = CategoryEntity.Create(
            name: "Women",
            slug: "women-fashion",
            description: "Women's fashion",
            parentId: fashion.Id);

        var home = CategoryEntity.Create(
            name: "Home & Living",
            slug: "home-living",
            description: "Household goods & furniture");

        return
        [
            electronics,
            phones,
            laptops,
            fashion,
            menFashion,
            womenFashion,
            home
        ];
    }
}
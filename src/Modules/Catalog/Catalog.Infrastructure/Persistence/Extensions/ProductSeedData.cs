using Catalog.Domain.Entities;

namespace Catalog.Infrastructure.Persistence;


public static class ProductSeedData
{
    public static ProductEntity[] GetAllProducts()
    {
        var electronics = CategorySeedData.GetCategories();

        var phones = electronics.First(x => x.Slug == "phones");
        var laptops = electronics.First(x => x.Slug == "laptops");
        var menFashion = electronics.First(x => x.Slug == "men-fashion");
        var womenFashion = electronics.First(x => x.Slug == "women-fashion");
        var fashion = electronics.First(x => x.Slug == "fashion");
        var home = electronics.First(x => x.Slug == "home-living");
        var electronicsCategory = electronics.First(x => x.Slug == "electronics");

        var iphone = ProductEntity.Create(
            name: "iPhone 15 Pro",
            slug: "iphone-15-pro",
            description: "Latest iPhone with A17 Pro chip and titanium design",
            price: 29990000,
            thumbnailUrl: GetThumbnailUrl("iPhone 15 Pro"),
            galleryImageUrls: GetProductImages("iPhone 15 Pro"),
            categoryIds: [phones.Id, electronicsCategory.Id]);

        var samsung = ProductEntity.Create(
            name: "Samsung Galaxy S24 Ultra",
            slug: "samsung-galaxy-s24-ultra",
            description: "Premium Android flagship with S Pen and advanced AI features",
            price: 27990000,
            thumbnailUrl: GetThumbnailUrl("Samsung Galaxy S24 Ultra"),
            galleryImageUrls: GetProductImages("Samsung Galaxy S24 Ultra"),
            categoryIds: [phones.Id, electronicsCategory.Id]);

        var macbook = ProductEntity.Create(
            name: "MacBook Pro 16-inch M3 Pro",
            slug: "macbook-pro-16-inch-m3-pro",
            description: "Powerful laptop for professionals with M3 Pro chip",
            price: 59990000,
            thumbnailUrl: GetThumbnailUrl("MacBook Pro 16-inch M3 Pro"),
            galleryImageUrls: GetProductImages("MacBook Pro 16-inch M3 Pro"),
            categoryIds: [laptops.Id, electronicsCategory.Id]);

        var dell = ProductEntity.Create(
            name: "Dell XPS 15",
            slug: "dell-xps-15",
            description: "Premium Windows laptop with OLED display",
            price: 45990000,
            thumbnailUrl: GetThumbnailUrl("Dell XPS 15"),
            galleryImageUrls: GetProductImages("Dell XPS 15"),
            categoryIds: [laptops.Id, electronicsCategory.Id]);

        var shirt = ProductEntity.Create(
            name: "Áo Sơ Mi Nam Cổ Điển",
            slug: "ao-so-mi-nam-co-dien",
            description: "Áo sơ mi nam cổ điển chất liệu cotton cao cấp",
            price: 890000,
            thumbnailUrl: GetThumbnailUrl("Áo Sơ Mi Nam Cổ Điển"),
            galleryImageUrls: GetProductImages("Áo Sơ Mi Nam Cổ Điển"),
            categoryIds: [menFashion.Id, fashion.Id]);

        var jeans = ProductEntity.Create(
            name: "Quần Jeans Nữ Skinny",
            slug: "quan-jeans-nu-skinny",
            description: "Quần jeans nữ dáng skinny co giãn cao cấp",
            price: 1290000,
            thumbnailUrl: GetThumbnailUrl("Quần Jeans Nữ Skinny"),
            galleryImageUrls: GetProductImages("Quần Jeans Nữ Skinny"),
            categoryIds: [womenFashion.Id, fashion.Id]);

        var jacket = ProductEntity.Create(
            name: "Áo Khoác Nam Bomber",
            slug: "ao-khoac-nam-bomber",
            description: "Áo khoác nam bomber phong cách streetwear",
            price: 1890000,
            thumbnailUrl: GetThumbnailUrl("Áo Khoác Nam Bomber"),
            galleryImageUrls: GetProductImages("Áo Khoác Nam Bomber"),
            categoryIds: [menFashion.Id, fashion.Id]);

        var sneakers = ProductEntity.Create(
            name: "Giày Thể Thao Nữ",
            slug: "giay-the-thao-nu",
            description: "Giày thể thao nữ đế cao su êm ái",
            price: 1590000,
            thumbnailUrl: GetThumbnailUrl("Giày Thể Thao Nữ"),
            galleryImageUrls: GetProductImages("Giày Thể Thao Nữ"),
            categoryIds: [womenFashion.Id, fashion.Id]);

        var handbag = ProductEntity.Create(
            name: "Túi Xách Gucci",
            slug: "tui-xach-gucci",
            description: "Túi xách cao cấp Gucci da thật",
            price: 45900000,
            thumbnailUrl: GetThumbnailUrl("Túi Xách Gucci"),
            galleryImageUrls: GetProductImages("Túi Xách Gucci"),
            categoryIds: [womenFashion.Id, fashion.Id]);

        var desk = ProductEntity.Create(
            name: "Bàn Làm Việc Gỗ",
            slug: "ban-lam-viec-go",
            description: "Bàn làm việc gỗ tự nhiên thiết kế hiện đại",
            price: 4590000,
            thumbnailUrl: GetThumbnailUrl("Bàn Làm Việc Gỗ"),
            galleryImageUrls: GetProductImages("Bàn Làm Việc Gỗ"),
            categoryIds: [home.Id]);

        var chair = ProductEntity.Create(
            name: "Ghế Văn Phòng Ergonomic",
            slug: "ghe-van-phong-ergonomic",
            description: "Ghế văn phòng ergonomic hỗ trợ lưng tốt",
            price: 3290000,
            thumbnailUrl: GetThumbnailUrl("Ghế Văn Phòng Ergonomic"),
            galleryImageUrls: GetProductImages("Ghế Văn Phòng Ergonomic"),
            categoryIds: [home.Id]);

        var lamp = ProductEntity.Create(
            name: "Đèn Bàn LED",
            slug: "den-ban-led",
            description: "Đèn bàn LED điều chỉnh độ sáng",
            price: 890000,
            thumbnailUrl: GetThumbnailUrl("Đèn Bàn LED"),
            galleryImageUrls: GetProductImages("Đèn Bàn LED"),
            categoryIds: [home.Id]);

        var bookshelf = ProductEntity.Create(
            name: "Tủ Sách 5 Tầng",
            slug: "tu-sach-5-tang",
            description: "Tủ sách 5 tầng gỗ công nghiệp",
            price: 2490000,
            thumbnailUrl: GetThumbnailUrl("Tủ Sách 5 Tầng"),
            galleryImageUrls: GetProductImages("Tủ Sách 5 Tầng"),
            categoryIds: [home.Id]);

        var xiaomi = ProductEntity.Create(
            name: "Xiaomi Redmi Note 13 Pro",
            slug: "xiaomi-redmi-note-13-pro",
            description: "Smartphone giá rẻ hiệu năng cao",
            price: 7990000,
            thumbnailUrl: GetThumbnailUrl("Xiaomi Redmi Note 13 Pro"),
            galleryImageUrls: GetProductImages("Xiaomi Redmi Note 13 Pro"),
            categoryIds: [phones.Id, electronicsCategory.Id]);

        var tshirt = ProductEntity.Create(
            name: "Áo Thun Nam Cổ Tròn",
            slug: "ao-thun-nam-co-tron",
            description: "Áo thun nam cổ tròn chất liệu cotton",
            price: 490000,
            thumbnailUrl: GetThumbnailUrl("Áo Thun Nam Cổ Tròn"),
            galleryImageUrls: GetProductImages("Áo Thun Nam Cổ Tròn"),
            categoryIds: [menFashion.Id, fashion.Id]);

        return
        [
            iphone,
        samsung,
        macbook,
        dell,
        shirt,
        jeans,
        jacket,
        sneakers,
        handbag,
        desk,
        chair,
        lamp,
        bookshelf,
        xiaomi,
        tshirt
        ];
    }



    public static string GetThumbnailUrl(string productName)
    {
        return productName.ToLower() switch
        {
            "iphone 15 pro" => "https://images.unsplash.com/photo-1592750475338-74b7b21085ab?w=800&h=800&fit=crop",
            "samsung galaxy s24 ultra" => "https://images.unsplash.com/photo-1610945265064-0e34e5519bbf?w=800&h=800&fit=crop",
            "macbook pro 16-inch m3 pro" => "https://images.unsplash.com/photo-1541807084-5c52b6b3adef?w=800&h=800&fit=crop",
            "dell xps 15" => "https://images.unsplash.com/photo-1496181133206-80ce9b88a853?w=800&h=800&fit=crop",
            "áo sơ mi nam cổ điển" => "https://images.unsplash.com/photo-1642764873654-9eef0467b342?w=800&h=800&fit=crop",
            "quần jeans nữ skinny" => "https://images.unsplash.com/photo-1542272604-787c3835535d?w=800&h=800&fit=crop",
            "áo khoác nam bomber" => "https://images.unsplash.com/photo-1551028719-00167b16eac5?w=800&h=800&fit=crop",
            "giày thể thao nữ" => "https://images.unsplash.com/photo-1542291026-7eec264c27ff?w=800&h=800&fit=crop",
            "túi xách gucci" => "https://images.unsplash.com/photo-1590874103328-eac38a683ce7?w=800&h=800&fit=crop",
            "bàn làm việc gỗ" => "https://images.unsplash.com/photo-1586023492125-27b2c045efd7?w=800&h=800&fit=crop",
            "ghế văn phòng ergonomic" => "https://images.unsplash.com/photo-1506439773649-6e0eb8cfb237?w=800&h=800&fit=crop",
            "đèn bàn led" => "https://images.unsplash.com/photo-1507473885765-e6ed057f782c?w=800&h=800&fit=crop",
            "tủ sách 5 tầng" => "https://images.unsplash.com/photo-1586023492125-27b2c045efd7?w=800&h=800&fit=crop",
            "xiaomi redmi note 13 pro" => "https://images.unsplash.com/photo-1511707171634-5f897ff02aa9?w=800&h=800&fit=crop",
            "áo thun nam cổ tròn" => "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?w=800&h=800&fit=crop",
            _ => "https://images.unsplash.com/photo-1441986300917-64674bd600d8?w=800&h=800&fit=crop"
        };
    }

    public static List<string> GetProductImages(string productName)
    {
        return productName.ToLower() switch
        {
            "iphone 15 pro" => new List<string>
            {
                "https://images.unsplash.com/photo-1592750475338-74b7b21085ab?w=1200&h=1200&fit=crop",
                "https://images.unsplash.com/photo-1511707171634-5f897ff02aa9?w=1200&h=1200&fit=crop",
                "https://images.unsplash.com/photo-1556656793-08538906a9f8?w=1200&h=1200&fit=crop"
            },
            "samsung galaxy s24 ultra" => new List<string>
            {
                "https://images.unsplash.com/photo-1610945265064-0e34e5519bbf?w=1200&h=1200&fit=crop",
                "https://images.unsplash.com/photo-1511707171634-5f897ff02aa9?w=1200&h=1200&fit=crop",
                "https://images.unsplash.com/photo-1556656793-08538906a9f8?w=1200&h=1200&fit=crop"
            },
            "macbook pro 16-inch m3 pro" => new List<string>
            {
                "https://images.unsplash.com/photo-1541807084-5c52b6b3adef?w=1200&h=1200&fit=crop",
                "https://images.unsplash.com/photo-1496181133206-80ce9b88a853?w=1200&h=1200&fit=crop",
                "https://images.unsplash.com/photo-1525547719571-a2d4ac8945e2?w=1200&h=1200&fit=crop"
            },
            "dell xps 15" => new List<string>
            {
                "https://images.unsplash.com/photo-1496181133206-80ce9b88a853?w=1200&h=1200&fit=crop",
                "https://images.unsplash.com/photo-1541807084-5c52b6b3adef?w=1200&h=1200&fit=crop",
                "https://images.unsplash.com/photo-1525547719571-a2d4ac8945e2?w=1200&h=1200&fit=crop"
            },
            "áo sơ mi nam cổ điển" => new List<string>
            {
                "https://images.unsplash.com/photo-1594938291221-94f18b6fa0e1?w=1200&h=1200&fit=crop",
                "https://images.unsplash.com/photo-1622445275576-721325763afe?w=1200&h=1200&fit=crop",
                "https://images.unsplash.com/photo-1603252109303-2751441dd157?w=1200&h=1200&fit=crop"
            },
            "quần jeans nữ skinny" => new List<string>
            {
                "https://images.unsplash.com/photo-1542272604-787c3835535d?w=1200&h=1200&fit=crop",
                "https://images.unsplash.com/photo-1582418702059-97ebafbcdb1d?w=1200&h=1200&fit=crop",
                "https://images.unsplash.com/photo-1541099649105-f69ad21f3246?w=1200&h=1200&fit=crop"
            },
            "áo khoác nam bomber" => new List<string>
            {
                "https://images.unsplash.com/photo-1551028719-00167b16eac5?w=1200&h=1200&fit=crop",
                "https://images.unsplash.com/photo-1539533018447-63fcce2678e3?w=1200&h=1200&fit=crop",
                "https://images.unsplash.com/photo-1556821840-3a63f95609a7?w=1200&h=1200&fit=crop"
            },
            "giày thể thao nữ" => new List<string>
            {
                "https://images.unsplash.com/photo-1542291026-7eec264c27ff?w=1200&h=1200&fit=crop",
                "https://images.unsplash.com/photo-1460353581641-37baddab0fa2?w=1200&h=1200&fit=crop",
                "https://images.unsplash.com/photo-1606107557195-0e29a4b5b4aa?w=1200&h=1200&fit=crop"
            },
            "túi xách gucci" => new List<string>
            {
                "https://images.unsplash.com/photo-1590874103328-eac38a683ce7?w=1200&h=1200&fit=crop",
                "https://images.unsplash.com/photo-1553062407-98eeb64c6a62?w=1200&h=1200&fit=crop",
                "https://images.unsplash.com/photo-1594223274512-ad4803739b7c?w=1200&h=1200&fit=crop"
            },
            "bàn làm việc gỗ" => new List<string>
            {
                "https://images.unsplash.com/photo-1586023492125-27b2c045efd7?w=1200&h=1200&fit=crop",
                "https://images.unsplash.com/photo-1532372320572-cda25653a26d?w=1200&h=1200&fit=crop",
                "https://images.unsplash.com/photo-1581539250439-c96689b516dd?w=1200&h=1200&fit=crop"
            },
            "ghế văn phòng ergonomic" => new List<string>
            {
                "https://images.unsplash.com/photo-1506439773649-6e0eb8cfb237?w=1200&h=1200&fit=crop",
                "https://images.unsplash.com/photo-1586023492125-27b2c045efd7?w=1200&h=1200&fit=crop",
                "https://images.unsplash.com/photo-1532372320572-cda25653a26d?w=1200&h=1200&fit=crop"
            },
            "đèn bàn led" => new List<string>
            {
                "https://images.unsplash.com/photo-1507473885765-e6ed057f782c?w=1200&h=1200&fit=crop",
                "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=1200&h=1200&fit=crop",
                "https://images.unsplash.com/photo-1526170375885-4d8ecf77b99f?w=1200&h=1200&fit=crop"
            },
            "tủ sách 5 tầng" => new List<string>
            {
                "https://images.unsplash.com/photo-1586023492125-27b2c045efd7?w=1200&h=1200&fit=crop",
                "https://images.unsplash.com/photo-1532372320572-cda25653a26d?w=1200&h=1200&fit=crop",
                "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=1200&h=1200&fit=crop"
            },
            "xiaomi redmi note 13 pro" => new List<string>
            {
                "https://images.unsplash.com/photo-1511707171634-5f897ff02aa9?w=1200&h=1200&fit=crop",
                "https://images.unsplash.com/photo-1592750475338-74b7b21085ab?w=1200&h=1200&fit=crop",
                "https://images.unsplash.com/photo-1556656793-08538906a9f8?w=1200&h=1200&fit=crop"
            },
            "áo thun nam cổ tròn" => new List<string>
            {
                "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?w=1200&h=1200&fit=crop",
                "https://images.unsplash.com/photo-1594938291221-94f18b6fa0e1?w=1200&h=1200&fit=crop",
                "https://images.unsplash.com/photo-1622445275576-721325763afe?w=1200&h=1200&fit=crop"
            },
            _ => new List<string>
            {
                "https://images.unsplash.com/photo-1441986300917-64674bd600d8?w=1200&h=1200&fit=crop",
                "https://images.unsplash.com/photo-1441986300917-64674bd600d8?w=1200&h=1200&fit=crop",
                "https://images.unsplash.com/photo-1441986300917-64674bd600d8?w=1200&h=1200&fit=crop"
            }
        };
    }
}


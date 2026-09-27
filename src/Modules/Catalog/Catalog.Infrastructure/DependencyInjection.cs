using Minio;
using Catalog.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Catalog.Infrastructure.Persistence.Extensions;
using Catalog.Infrastructure.FileStorages;
using Microsoft.Extensions.Options;
using Catalog.Application.Abstractions;
using Catalog.Domain.Repositories;
using Catalog.Infrastructure.Persistence.Services;
using Catalog.Application.Features.Product.Services;
using Catalog.Application.Features.Category.Services;

namespace Catalog.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration cfg)
    {
        services.AddDbContext<CatalogDbContext>(cfg);

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IProductRepository, ProductRepository>();

        services.AddScoped<ICategoryRepository, CategoryRepository>();

        services.AddScoped<ICategoryQueryService, CategoryQueryService>();

        services.AddScoped<IProductQueryService, ProductQueryService>();

        services.AddScoped<ICatalogModule, CatalogModule>();

        services.Configure<MinioOptions>(cfg.GetSection(MinioOptions.SectionName));

        var minioOpts = cfg
            .GetSection(MinioOptions.SectionName)
            .Get<MinioOptions>()
            ?? throw new InvalidOperationException("MinIO configuration is missing.");

        services.AddSingleton<IMinioClient>(sp =>
        {
            var opts = sp.GetRequiredService<IOptions<MinioOptions>>().Value;

            var client = new MinioClient()
                .WithEndpoint(opts.Endpoint)
                .WithCredentials(
                    opts.AccessKey,
                    opts.SecretKey);

            if (opts.UseSsl)
            {
                client = client.WithSSL();
            }

            return client.Build();
        });

        services.AddHttpClient();

        services.AddScoped<IFileStorageService, MinioFileStorageService>();

        return services;
    }

    public static WebApplication UseInfrastructure(this WebApplication app)
    {
        app.InitialiseDatabaseAsync().GetAwaiter().GetResult();
        return app;
    }
}

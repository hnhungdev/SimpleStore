using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Catalog.Application;
using Catalog.Infrastructure;
using Microsoft.AspNetCore.Builder;

namespace Catalog.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddCatalogModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddApplicationServices()
            .AddInfrastructureServices(configuration);

        return services;
    }

    public static WebApplication UseCatalogModule(this WebApplication app)
    {
        app.UseInfrastructure();

        return app;
    }
}
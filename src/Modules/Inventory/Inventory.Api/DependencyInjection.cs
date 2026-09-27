using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Inventory.Application;
using Inventory.Infrastructure;
using Microsoft.AspNetCore.Builder;

namespace Inventory.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddInventoryModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddApplicationServices()
            .AddInfrastructureServices(configuration);

        return services;
    }

    public static WebApplication UseInventoryModule(this WebApplication app)
    {
        app.UseInfrastructure();

        return app;
    }
}
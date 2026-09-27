using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Cart.Application;
using Cart.Infrastructure;

namespace Cart.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddCartModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddApplicationServices()
            .AddInfrastructureServices(configuration);

        return services;
    }
}
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Order.Application;
using Order.Infrastructure;

namespace Order.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddOrderModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddApplicationServices()
            .AddInfrastructureServices(configuration);

        return services;
    }
}
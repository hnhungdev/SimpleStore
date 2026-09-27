using Cart.Domain.Repositories;
using Cart.Application.Abstractions;
using Cart.Infrastructure.Persistence;
using Cart.Infrastructure.Repositories;
using Cart.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Cart.Infrastructure.Persistence.Services;
using Cart.Application.Features.Cart.Services;

namespace Cart.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration cfg)
    {
        services.AddDbContext<CartDbContext>(cfg);

        services.AddScoped<ICartRepository, CartRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<ICartCache, CartCache>();

        services.AddScoped<ICartQueryService, CartQueryService>();

        services.Decorate<ICartQueryService, CachedCartQueryService>();

        services.AddScoped<ICartModule, CartModule>();

        services.AddStackExchangeRedisCache(options =>
        {
            options.ConfigurationOptions = new ConfigurationOptions
            {
                EndPoints = { cfg[$"{RedisCacheCfg.Section}:{RedisCacheCfg.EndPoint}"]! },
                Password = cfg[$"{RedisCacheCfg.Section}:{RedisCacheCfg.Password}"]!,
                AbortOnConnectFail = false,
                ConnectRetry = 3,
                ConnectTimeout = 5000,
                DefaultDatabase = 0
            };
            options.InstanceName = cfg[$"{RedisCacheCfg.Section}:{RedisCacheCfg.InstanceName}"]!;
        });

        return services;
    }
}

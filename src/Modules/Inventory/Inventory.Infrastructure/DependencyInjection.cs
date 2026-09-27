using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Inventory.Domain.Repositories;
using Inventory.Application.Abstractions;
using Inventory.Application.Features.InventoryItem.Services;
using Inventory.Infrastructure.Persistence;
using Inventory.Infrastructure.Persistence.Repositories;
using Inventory.Infrastructure.Persistence.Services;
using Inventory.Infrastructure.Messaging.Outbox;
using Inventory.Infrastructure.Messaging.Inbox;

namespace Inventory.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration cfg)
    {
        services.AddDbContext<InventoryDbContext>(cfg);

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IInventoryItemRepository, InventoryItemRepository>();

        services.AddScoped<IInventoryReservationRepository, InventoryReservationRepository>();

        services.AddScoped<IInventoryTransactionRepository, InventoryTransactionRepository>();

        services.AddScoped<IInventoryQueryService, InventoryQueryService>();

        services.AddScoped<IInventoryModule, InventoryModule>();

        services.AddScoped<IInventoryInboxStore, InventoryInboxStore>();

        services.AddScoped<IInventoryOutboxStore, InventoryOutboxStore>();

        services.AddHostedService<OutboxProcessorBackgroundService<InventoryDbContext>>();

        return services;
    }

    public static WebApplication UseInfrastructure(this WebApplication app)
    {
        return app;
    }
}
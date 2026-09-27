using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notification.Api.Hubs;
using Notification.Api.Services;
using Notification.Application;
using Notification.Application.Abstractions;
using Notification.Infrastructure;

namespace Notification.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddNotificationModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddApplicationServices()
            .AddInfrastructureServices(configuration);

        services.AddScoped<INotificationHubService, NotificationHubService>();

        services.AddSignalR();

        return services;
    }

    public static WebApplication UseNotificationModule(this WebApplication app)
    {
        app.UseInfrastructure();

        app.MapHub<NotificationHub>("/hubs/notifications");

        return app;
    }
}
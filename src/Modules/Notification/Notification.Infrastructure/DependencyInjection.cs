using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notification.Application.Abstractions;
using Notification.Application.Features.Notification.Services;
using Notification.Application.Providers;
using Notification.Application.Strategy;
using Notification.Domain.Repositories;
using Notification.Infrastructure.Messaging.Inbox;
using Notification.Infrastructure.Persistence;
using Notification.Infrastructure.Persistence.Extensions;
using Notification.Infrastructure.Persistence.Repositories;
using Notification.Infrastructure.Persistence.Services;
using Notification.Infrastructure.Providers;
using Notification.Infrastructure.Resolvers;
using Notification.Infrastructure.Senders;

namespace Notification.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration cfg)
    {
        services.AddDbContext<NotificationDbContext>(cfg);

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<INotificationJobRepository, NotificationJobRepository>();

        services.AddScoped<IUserNotificationRepository, UserNotificationRepository>();

        services.AddScoped<INotificationTemplateRepository, NotificationTemplateRepository>();

        services.AddScoped<IUserNotificationQueryService, UserNotificationQueryService>();

        services.AddSingleton<ITemplateProvider, TemplateProvider>();

        services.AddScoped<INotificationSenderResolver, NotificationChannelResolver>();

        services.AddScoped<INotificationSender, EmailNotificationSender>();

        services.AddScoped<INotificationSender, InAppNotificationSender>();

        services.AddScoped<INotificationInboxStore, NotificationInboxStore>();

        services.AddHostedService<NotificationWorker>();

        return services;
    }

    public static WebApplication UseInfrastructure(this WebApplication app)
    {
        app.InitialiseDatabaseAsync().GetAwaiter().GetResult();

        return app;
    }
}

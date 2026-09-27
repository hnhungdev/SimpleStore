using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Payment.Domain.Repositories;
using Payment.Infrastructure.Persistence;
using Payment.Infrastructure.Payments;
using Payment.Infrastructure.Payments.Momo;
using Payment.Infrastructure.Payments.Vnpay;
using Payment.Infrastructure.Persistence.Repositories;
using Payment.Infrastructure.Payments.Cod;
using Payment.Application.Abstractions;
using Payment.Infrastructure.Messaging.Inbox;
using Payment.Infrastructure.Messaging.Outbox;

namespace Payment.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration cfg)
    {

        services.AddDbContext<PaymentDbContext>(cfg);

        services.AddScoped<IPaymentRepository, PaymentRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.Configure<MomoOptions>(cfg.GetSection(MomoOptions.SectionName));

        services.Configure<VnpayOptions>(cfg.GetSection(VnpayOptions.SectionName));

        services.AddScoped<IPaymentServiceFactory, PaymentServiceFactory>();

        services.AddScoped<ICurrentRequestContext, CurrentRequestContext>();

        services.AddScoped<IPaymentService, VnpayPaymentService>();

        services.AddScoped<IPaymentService, MomoPaymentService>();

        services.AddScoped<IPaymentService, CodPaymentService>();

        services.AddScoped<IPaymentInboxStore, PaymentInboxStore>();

        services.AddScoped<IPaymentOutboxStore, PaymentOutboxStore>();

        services.AddHostedService<OutboxProcessorBackgroundService<PaymentDbContext>>();

        services.AddHttpClient();

        return services;
    }
}

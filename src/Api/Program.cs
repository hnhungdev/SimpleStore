using MassTransit;
using Carter;
using Cart.Api;
using Catalog.Api;
using Inventory.Api;
using Notification.Api;
using Order.Api;
using Payment.Api;
using Identity.Api;
using Order.Application.Sagas;
using Order.Infrastructure.Persistence;
using Identity.Api.Extensions;
using Api.ExceptionHandlers;
using Api.Extensions;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCarter();
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddScoped<ICurrentUser, CurrentUser>();

builder.Services.Configure<MessageBrokerOptions>(
    builder.Configuration.GetSection(MessageBrokerOptions.SectionName));

var options = builder.Configuration
    .GetSection(MessageBrokerOptions.SectionName)
    .Get<MessageBrokerOptions>()
    ?? throw new InvalidOperationException("Message broker configuration is missing.");

builder.Services.AddSingleton<CheckoutSagaDefinition>();

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();

    x.AddConsumers(
        typeof(Order.Infrastructure.DependencyInjection).Assembly,
        typeof(Inventory.Infrastructure.DependencyInjection).Assembly,
        typeof(Payment.Infrastructure.DependencyInjection).Assembly,
        typeof(Notification.Infrastructure.DependencyInjection).Assembly);

    x.AddEntityFrameworkOutbox<OrderDbContext>(o =>
    {
        o.UseSqlServer();
        o.UseBusOutbox();
    });

    x.AddDelayedMessageScheduler();

    x.AddSagaStateMachine<
        CheckoutSagaStateMachine,
        CheckoutSagaState,
        CheckoutSagaDefinition>()
        .EntityFrameworkRepository(r =>
        {
            r.ConcurrencyMode = ConcurrencyMode.Optimistic;
            r.ExistingDbContext<OrderDbContext>();
            r.UseSqlServer();
        });

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(options.Host, "/", h =>
        {
            h.Username(options.UserName);
            h.Password(options.Password);
        });

        cfg.UseDelayedMessageScheduler();
        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    // Sensitive endpoints: 10 requests / 10s / IP
    options.AddSlidingWindowLimiter("sliding", opt =>
    {
        opt.PermitLimit = 10;
        opt.Window = TimeSpan.FromSeconds(10);
        opt.SegmentsPerWindow = 2;
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 2;
    });

    // Global: 100 requests / 1 minute / IP
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(
        context => RateLimitPartition.GetFixedWindowLimiter(
            context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 100,
                Window = TimeSpan.FromMinutes(1),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 5
            }));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("Cors", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:5173",
                "http://localhost:3001",
                "http://localhost:3002")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// Đăng ký các Module
builder.Services.AddCatalogModule(builder.Configuration);
builder.Services.AddCartModule(builder.Configuration);
builder.Services.AddOrderModule(builder.Configuration);
builder.Services.AddInventoryModule(builder.Configuration);
builder.Services.AddPaymentModule(builder.Configuration);
builder.Services.AddNotificationModule(builder.Configuration);
builder.Services.AddIdentityModule(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler(options => { });
app.UseAuthentication();
app.UseAuthorization();

app.MapCarter();

app.UseCors("Cors");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "/swagger/v1/swagger.json",
            "SimpleStore API");
    });
}

app.UseInventoryModule();
app.UseNotificationModule();
app.UseCatalogModule();
app.UseIdentityModule();

app.Run();
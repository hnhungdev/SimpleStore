using Identity.Api.Data;
using Identity.Api.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Identity.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Builder;
using Identity.Api.Extensions;

namespace Identity.Api;

public static class DependecyInjection
{
    public static IServiceCollection AddIdentityModule(this IServiceCollection services, IConfiguration cfg)
    {
        services.AddDbContext<IdentityDbContext>((sp, options) =>
        {
            options.UseSqlServer(cfg.GetConnectionString("DefaultConnection"));
        });

        services.Configure<JwtOptions>(cfg.GetSection("Jwt"));
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        services
            .AddIdentityCore<ApplicationUser>(options =>
            {
                options.User.RequireUniqueEmail = true;
            })
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<IdentityDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

        return services;
    }

    public static WebApplication UseIdentityModule(this WebApplication app)
    {
        app.InitialiseDatabaseAsync().GetAwaiter().GetResult();
        return app;
    }
}
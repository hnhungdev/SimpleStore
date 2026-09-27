using Identity.Api.Data;
using Identity.Api.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Api.Extensions;

public static class DatabaseExtentions
{
    public static async Task InitialiseDatabaseAsync(
        this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var services = scope.ServiceProvider;

        var dbContext =
            services.GetRequiredService<IdentityDbContext>();

        var userManager =
            services.GetRequiredService<UserManager<ApplicationUser>>();

        var roleManager =
            services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        await IdentitySeeder.SeedAsync(
            dbContext,
            userManager,
            roleManager);
    }
}

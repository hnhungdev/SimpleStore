using Identity.Api.Data;
using Identity.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.Api.Extensions;

public static class IdentitySeeder
{
    public const string DemoUserEmail = "demo@simplestore.local";
    public const string DemoUserPassword = "Demo123!";

    public const string AdminUserEmail = "admin@simplestore.local";
    public const string AdminUserPassword = "Admin123!";

    public const string AdminRole = "Admin";
    public const string CustomerRole = "Customer";

    public static async Task SeedAsync(
        IdentityDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole<Guid>> roleManager)
    {
        if (await context.Users.AnyAsync())
            return;

        foreach (var role in new[] { AdminRole, CustomerRole })
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(
                    new IdentityRole<Guid>(role));
            }
        }

        await EnsureUserAsync(
            userManager,
            AdminUserEmail,
            AdminUserPassword,
            "Site Admin",
            AdminRole);

        await EnsureUserAsync(
            userManager,
            DemoUserEmail,
            DemoUserPassword,
            "Demo User",
            CustomerRole);
    }

    private static async Task EnsureUserAsync(
        UserManager<ApplicationUser> userManager,
        string email,
        string password,
        string fullName,
        string role)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user is null)
        {
            user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                FullName = fullName
            };

            var create = await userManager.CreateAsync(
                user,
                password);

            if (!create.Succeeded)
                return;
        }

        if (!await userManager.IsInRoleAsync(user, role))
        {
            await userManager.AddToRoleAsync(user, role);
        }
    }
}
using System.Security.Claims;
using Carter;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Identity.Api.Services;
using Identity.Api.Dtos;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Identity.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Endpoints;

public sealed class IdentityEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var authGroup = app.MapGroup("/auth")
            .WithTags("Authentication")
            .RequireRateLimiting("sliding");

        authGroup.MapPost("/register", RegisterAsync);

        authGroup.MapPost("/login", LoginAsync);

        authGroup.MapPost("/refresh", RefreshAsync);

        authGroup.MapPost("/logout", LogoutAsync);

        var userGroup = app.MapGroup("/users")
            .WithTags("Users")
            .RequireAuthorization();

        userGroup.MapGet("/me", GetProfileAsync);

        userGroup.MapPut("/me", UpdateProfileAsync);

        var adminUserGroup = app.MapGroup("/admin/users")
            .WithTags("Admin - Users")
            .RequireAuthorization("Admin");

        adminUserGroup.MapGet("", GetUsersAsync);

        adminUserGroup.MapGet("/{id:guid}", GetUserAsync);

        adminUserGroup.MapPost("/{id:guid}/lock", LockUserAsync);

        adminUserGroup.MapPost("/{id:guid}/unlock", UnlockUserAsync);
    }

    private static async Task<IResult> RegisterAsync(
        [FromBody] RegisterRequest request,
        [FromServices] IIdentityService service,
        CancellationToken ct)
    {
        var response = await service.RegisterAsync(request, ct);

        return response is null
            ? Results.BadRequest(new
            {
                error = "Could not register user."
            })
            : Results.Ok(response);
    }

    private static async Task<IResult> LoginAsync(
        [FromBody] LoginRequest request,
        [FromServices] IIdentityService service,
        CancellationToken ct)
    {
        var response = await service.LoginAsync(request, ct);

        return response is null
            ? Results.Unauthorized()
            : Results.Ok(response);
    }

    private static async Task<IResult> RefreshAsync(
        [FromBody] RefreshRequest request,
        [FromServices] IIdentityService service,
        CancellationToken ct)
    {
        var response = await service.RefreshAsync(request.RefreshToken, ct);

        return response is null
            ? Results.Unauthorized()
            : Results.Ok(response);
    }

    private static async Task<IResult> LogoutAsync(
        [FromBody] RefreshRequest request,
        [FromServices] IIdentityService service,
        CancellationToken ct)
    {
        await service.LogoutAsync(request.RefreshToken, ct);

        return Results.NoContent();
    }

    private static async Task<IResult> GetProfileAsync(
        [FromServices] IIdentityService service,
        ClaimsPrincipal principal,
        CancellationToken ct)
    {
        var userIdValue =
               principal.FindFirstValue("sub") ??
               principal.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdValue, out var userId))
            return Results.Unauthorized();

        var info = await service.GetUserInfoAsync(userId, ct);

        return info is null
            ? Results.NotFound()
            : Results.Ok(info);
    }

    private static async Task<IResult> UpdateProfileAsync(
        [FromBody] UpdateProfileRequest request,
        ClaimsPrincipal principal,
        [FromServices] UserManager<ApplicationUser> userManager,
        CancellationToken ct)
    {
        var userId =
            principal.FindFirstValue("sub") ??
            principal.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userId, out var userGuid))
            return Results.Unauthorized();

        var user = await userManager.FindByIdAsync(
            userGuid.ToString());

        if (user is null)
            return Results.NotFound();

        user.FullName = request.FullName?.Trim() ?? string.Empty;

        var update = await userManager.UpdateAsync(user);

        return update.Succeeded
            ? Results.NoContent()
            : Results.BadRequest(new
            {
                error = string.Join(
                    "; ",
                    update.Errors.Select(e => e.Description))
            });
    }

    private static async Task<IResult> GetUsersAsync(
        UserManager<ApplicationUser> userManager,
        int page = 1,
        int pageSize = 20,
        string? search = null,
        CancellationToken ct = default)
    {
        if (page < 1)
            page = 1;

        if (pageSize is < 1 or > 100)
            pageSize = 20;

        var query = userManager.Users.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim();

            query = query.Where(u =>
                (u.Email != null && u.Email.Contains(s)) ||
                u.FullName.Contains(s));
        }

        var total = await query.CountAsync(ct);

        var items = await query
            .OrderBy(u => u.Email)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        var summaries = new List<UserSummary>();

        foreach (var user in items)
        {
            var roles = await userManager.GetRolesAsync(user);

            summaries.Add(new UserSummary
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                FullName = user.FullName,
                LockoutEnd = user.LockoutEnd,
                IsLockedOut =
                    user.LockoutEnd is { } end &&
                    end > DateTimeOffset.UtcNow,
                Roles = roles.ToArray()
            });
        }

        return Results.Ok(new PagedResult<UserSummary>
        {
            Items = summaries,
            Page = page,
            PageSize = pageSize,
            TotalCount = total
        });
    }

    private static async Task<IResult> GetUserAsync(
        Guid id,
        UserManager<ApplicationUser> userManager,
        CancellationToken ct)
    {
        var user = await userManager.FindByIdAsync(
            id.ToString());

        if (user is null)
            return Results.NotFound();

        var roles = await userManager.GetRolesAsync(user);

        return Results.Ok(new UserSummary
        {
            Id = user.Id,
            Email = user.Email ?? string.Empty,
            FullName = user.FullName,
            Roles = roles.ToArray(),
            IsLockedOut =
                    user.LockoutEnd is { } end &&
                    end > DateTimeOffset.UtcNow,
        });
    }

    private static async Task<IResult> LockUserAsync(
        Guid id,
        UserManager<ApplicationUser> userManager,
        CancellationToken ct)
    {
        var user = await userManager.FindByIdAsync(
            id.ToString());

        if (user is null)
            return Results.NotFound();

        await userManager.SetLockoutEnabledAsync(
            user,
            true);

        var result = await userManager.SetLockoutEndDateAsync(
            user,
            DateTimeOffset.UtcNow.AddYears(100));

        return result.Succeeded
            ? Results.NoContent()
            : Results.BadRequest();
    }

    private static async Task<IResult> UnlockUserAsync(
        Guid id,
        UserManager<ApplicationUser> userManager,
        CancellationToken ct)
    {
        var user = await userManager.FindByIdAsync(
            id.ToString());

        if (user is null)
            return Results.NotFound();

        var result = await userManager.SetLockoutEndDateAsync(
            user,
            null);

        return result.Succeeded
            ? Results.NoContent()
            : Results.BadRequest();
    }
}
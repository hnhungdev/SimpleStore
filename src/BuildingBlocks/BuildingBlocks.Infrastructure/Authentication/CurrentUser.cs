using System.Security.Claims;
using BuildingBlocks.Application.Authentication;
using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.Infrastructure.Authentication;

public sealed class CurrentUser(
    IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    private readonly ClaimsPrincipal _user =
        httpContextAccessor.HttpContext?.User
        ?? new ClaimsPrincipal();

    public Guid UserId =>
        Guid.Parse(
            _user.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? _user.FindFirstValue("sub")
            ?? throw new UnauthorizedAccessException());

    public string Email =>
        _user.FindFirstValue(ClaimTypes.Email)!;

    public bool IsAuthenticated =>
        _user.Identity?.IsAuthenticated == true;

    public bool IsInRole(string role) =>
        _user.IsInRole(role);
}
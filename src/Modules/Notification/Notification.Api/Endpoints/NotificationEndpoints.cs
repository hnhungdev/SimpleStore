using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Notification.Application.Features.Notification.Commands;
using Notification.Application.Features.Notification.Dtos;
using Notification.Application.Features.Notification.Queries;

namespace Notification.Api.Endpoints;

public sealed class NotificationEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/notifications")
            .WithTags("Notifications")
            .RequireAuthorization();

        group.MapGet("/unread-count", GetCountNotificationUnreadAsync);

        group.MapGet("/recent", GetRecentNotificationsAsync);

        group.MapGet("/", GetNotificationsAsync);

        group.MapPatch("/read-all", MarkAllNotificationAsReadAsync);

        group.MapPatch("/{id:guid}/read", MarkNotificationAsReadAsync);

    }

    private static async Task<IResult> GetCountNotificationUnreadAsync(
        ISender sender,
        ICurrentUser currentUser,
        CancellationToken ct)
    {
        var query = new GetCountNotificationUnreadQuery(currentUser.UserId);

        var result = await sender.Send(query, ct);

        return Results.Ok(result);
    }

    private static async Task<IResult> GetRecentNotificationsAsync(
        ISender sender,
        ICurrentUser currentUser,
        CancellationToken ct)
    {
        var query = new GetRecentNotificationsQuery(currentUser.UserId);

        var result = await sender.Send(query, ct);

        return Results.Ok(result);
    }

    private static async Task<IResult> GetNotificationsAsync(
        ISender sender,
        ICurrentUser currentUser,
        [AsParameters] NotificationFilter filter,
        [AsParameters] PaginationRequest pagination,
        CancellationToken ct)
    {
        var query = new GetNotificationsQuery(currentUser.UserId, filter, pagination);

        var result = await sender.Send(query, ct);

        return Results.Ok(result);
    }

    private static async Task<IResult> MarkAllNotificationAsReadAsync(
        ISender sender,
        ICurrentUser currentUser,
        CancellationToken ct)
    {
        var command = new MarkAllNotificationAsReadCommand(currentUser.UserId);

        await sender.Send(command, ct);

        return Results.NoContent();
    }

    private static async Task<IResult> MarkNotificationAsReadAsync(
        ISender sender,
        ICurrentUser currentUser,
        Guid id,
        CancellationToken ct)
    {
        var command = new MarkNotificationAsReadCommand(currentUser.UserId, id);

        await sender.Send(command, ct);

        return Results.NoContent();
    }
}
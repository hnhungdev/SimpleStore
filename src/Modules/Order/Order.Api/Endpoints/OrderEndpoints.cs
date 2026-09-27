using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Order.Application.Features.Order.Commands;
using Order.Application.Features.Order.Dtos;
using Order.Application.Features.Order.Queries;

namespace Order.Api.Endpoints;

public sealed class OrderEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var groupAdmin = app.MapGroup("/admin/orders")
            .WithTags("Admin - Orders")
            .RequireAuthorization("Admin");

        groupAdmin.MapGet("/", GetOrdersAsync);

        groupAdmin.MapGet("/all", GetAllOrdersAsync);

        groupAdmin.MapGet("/{orderId:guid}", GetOrderByIdAsync);

        groupAdmin.MapPatch("/{orderId:guid}/status", UpdateOrderStatusAsync);

        groupAdmin.MapPatch("/{orderId:guid}/cancel", CancelOrderAsync);

        var group = app.MapGroup("/orders")
            .WithTags("Orders")
            .RequireAuthorization();

        group.MapGet("/", GetMyOrdersAsync);

        group.MapGet("/{orderId:guid}", GetMyOrderByIdAsync);

        group.MapPost("/", CreateMyOrderAsync);

        group.MapPut("/{orderId:guid}", UpdateMyOrderAsync);

        group.MapPatch("/{orderId:guid}/cancel", CancelMyOrderAsync);
    }

    private static async Task<IResult> GetOrdersAsync(
        ISender sender,
        [AsParameters] GetOrdersFilter filter,
        [AsParameters] PaginationRequest pagination,
        CancellationToken ct)
    {
        var query = new GetOrdersQuery(filter, pagination);

        var result = await sender.Send(query, ct);

        return Results.Ok(result);
    }

    private static async Task<IResult> GetAllOrdersAsync(
        ISender sender,
        [AsParameters] GetOrdersFilter filter,
        CancellationToken ct)
    {
        var query = new GetAllOrdersQuery(filter);

        var result = await sender.Send(query, ct);

        return Results.Ok(result);
    }

    private static async Task<IResult> GetOrderByIdAsync(
        ISender sender,
        [FromRoute] Guid orderId,
        CancellationToken ct)
    {
        var query = new GetOrderByIdQuery(orderId);

        var result = await sender.Send(query, ct);

        return Results.Ok(result);
    }

    private static async Task<IResult> UpdateOrderStatusAsync(
        ISender sender,
        [FromRoute] Guid orderId,
        [FromBody] UpdateOrderStatusDto dto,
        CancellationToken ct)
    {
        var command = new UpdateOrderStatusCommand(orderId, dto);

        await sender.Send(command, ct);

        return Results.NoContent();
    }

    private static async Task<IResult> CancelOrderAsync(
        ISender sender,
        [FromRoute] Guid orderId,
        [FromBody] CancelOrderDto dto,
        CancellationToken ct)
    {
        var command = new CancelOrderCommand(orderId, dto);

        await sender.Send(command, ct);

        return Results.NoContent();
    }

    private static async Task<IResult> GetMyOrdersAsync(
        ISender sender,
        ICurrentUser currentUser,
        [AsParameters] GetMyOrdersFilter filter,
        [AsParameters] PaginationRequest pagination,
        CancellationToken ct)
    {
        var query = new GetMyOrdersQuery(currentUser.UserId, filter, pagination);

        var result = await sender.Send(query, ct);

        return Results.Ok(result);
    }

    private static async Task<IResult> GetMyOrderByIdAsync(
        ISender sender,
        ICurrentUser currentUser,
        [FromRoute] Guid orderId,
        CancellationToken ct)
    {
        var query = new GetMyOrderByIdQuery(orderId, currentUser.UserId);

        var result = await sender.Send(query, ct);

        return Results.Ok(result);
    }

    private static async Task<IResult> CreateMyOrderAsync(
        ISender sender,
        ICurrentUser currentUser,
        [FromBody] CreateOrderDto dto,
        CancellationToken ct)
    {
        var command = new CreateMyOrderCommand(currentUser.UserId, dto);

        var result = await sender.Send(command, ct);

        return Results.Created($"/orders/{result}", result);
    }

    private static async Task<IResult> UpdateMyOrderAsync(
        ISender sender,
        ICurrentUser currentUser,
        [FromRoute] Guid orderId,
        [FromBody] UpdateOrderDto dto,
        CancellationToken ct)
    {
        var command = new UpdateMyOrderCommand(currentUser.UserId, orderId, dto);

        await sender.Send(command, ct);

        return Results.NoContent();
    }

    private static async Task<IResult> CancelMyOrderAsync(
        ISender sender,
        [FromRoute] Guid orderId,
        [FromBody] CancelOrderDto dto,
        ICurrentUser currentUser,
        CancellationToken ct)
    {
        var command = new CancelMyOrderCommand(currentUser.UserId, orderId, dto);

        await sender.Send(command, ct);

        return Results.NoContent();
    }
}
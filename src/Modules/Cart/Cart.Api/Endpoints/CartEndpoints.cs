using Cart.Application.Features.Cart.Queries;
using Cart.Application.Features.Cart.Commands;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Cart.Application.Features.Cart.Dtos;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Cart.Api.Endpoints;

public sealed class CartEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/cart")
            .WithTags("Carts")
            .RequireAuthorization();

        group.MapGet("/", GetCartAsync);

        group.MapPost("/items/{productId:guid}", AddCartItemAsync);

        group.MapDelete("/items/{productId:guid}", RemoveCartItemAsync);

        group.MapPut("/items/{productId:guid}", UpdateCartItemAsync);

        group.MapDelete("/", ClearCartAsync);
    }

    private static async Task<IResult> GetCartAsync(
        ISender sender,
        ICurrentUser currentUser,
        CancellationToken ct)
    {
        var query = new GetCartQuery(currentUser.UserId);

        var result = await sender.Send(query, ct);

        return Results.Ok(result);
    }

    private static async Task<IResult> AddCartItemAsync(
        ISender sender,
        ICurrentUser currentUser,
        [FromRoute] Guid productId,
        [FromBody] AddCartItemDto dto,
        CancellationToken ct)
    {
        var command = new AddCartItemCommand(currentUser.UserId, productId, dto);

        await sender.Send(command, ct);

        return Results.NoContent();
    }

    private static async Task<IResult> RemoveCartItemAsync(
        ISender sender,
        ICurrentUser currentUser,
        [FromRoute] Guid productId,
        CancellationToken ct)
    {
        var command = new RemoveCartItemCommand(currentUser.UserId, productId);

        await sender.Send(command, ct);

        return Results.NoContent();
    }

    private static async Task<IResult> UpdateCartItemAsync(
        ISender sender,
        ICurrentUser currentUser,
        [FromRoute] Guid productId,
        [FromBody] UpdateQuantityDto dto,
        CancellationToken ct)
    {
        var command = new UpdateCartItemCommand(currentUser.UserId, productId, dto);

        await sender.Send(command, ct);

        return Results.NoContent();
    }

    private static async Task<IResult> ClearCartAsync(
        ISender sender,
        ICurrentUser currentUser,
        CancellationToken ct)
    {
        var command = new ClearCartCommand(currentUser.UserId);

        await sender.Send(command, ct);

        return Results.NoContent();
    }
}
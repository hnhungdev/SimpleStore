using Carter;
using Inventory.Application.Features.InventoryItem.Commands;
using Inventory.Application.Features.InventoryItem.Queries;
using Inventory.Application.Features.InventoryItem.Dtos;
using Inventory.Application.Features.InventoryTransaction.Queries;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Inventory.Application.Features.InventoryReservation.Dtos;
using Inventory.Application.Features.InventoryReservation.Queries;

namespace Inventory.Api.Endpoints;

public sealed class InventoryEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/admin/inventory-items")
            .WithTags("Admin - Inventory")
            .RequireAuthorization("Admin");

        group.MapGet("", GetInventoryItemsAsync);

        group.MapPost("", CreateInventoryItemAsync);

        group.MapPatch("/{inventoryItemId:guid}/increase-stock", IncreaseStockAsync);

        group.MapPatch("/{inventoryItemId:guid}/decrease-stock", DecreaseStockAsync);

        group.MapGet("/{inventoryItemId:guid}/transactions", GetInventoryTransactionsAsync);

        var reservationGroup = app.MapGroup("/admin/reservations")
            .WithTags("Admin - Inventory")
            .RequireAuthorization("Admin");

        reservationGroup.MapGet("/", GetInventoryReservationsAsync);
    }

    private async Task<IResult> GetInventoryItemsAsync(
        ISender sender,
        [AsParameters] InventoryItemFilter filter,
        [AsParameters] PaginationRequest pagination,
        CancellationToken ct)
    {
        var query = new GetInventoryItemsQuery(filter, pagination);

        var result = await sender.Send(query, ct);

        return Results.Ok(result);
    }

    private async Task<IResult> CreateInventoryItemAsync(
        ISender sender,
        [FromBody] CreateInventoryItemDto dto,
        CancellationToken ct)
    {
        var command = new CreateInventoryItemCommand(dto);

        var result = await sender.Send(command, ct);

        return Results.Created($"/admin/inventory-items/{result}", result);
    }

    private async Task<IResult> IncreaseStockAsync(
        ISender sender,
        [FromRoute] Guid inventoryItemId,
        [FromBody] IncreaseStockDto dto,
        CancellationToken ct)
    {
        var command = new IncreaseStockCommand(inventoryItemId, dto.Quantity);

        await sender.Send(command, ct);

        return Results.NoContent();
    }

    private async Task<IResult> DecreaseStockAsync(
        ISender sender,
        [FromRoute] Guid inventoryItemId,
        [FromBody] DecreaseStockDto dto,
        CancellationToken ct)
    {
        var command = new DecreaseStockCommand(inventoryItemId, dto.Quantity);

        await sender.Send(command, ct);

        return Results.NoContent();
    }

    private async Task<IResult> GetInventoryTransactionsAsync(
        ISender sender,
        [FromRoute] Guid inventoryItemId,
        [AsParameters] PaginationRequest pagination,
        CancellationToken ct)
    {
        var query = new GetInventoryTransactionsQuery(inventoryItemId, pagination);

        var result = await sender.Send(query, ct);

        return Results.Ok(result);
    }

    private async Task<IResult> GetInventoryReservationsAsync(
        ISender sender,
        [AsParameters] InventoryReservationFilter filter,
        [AsParameters] PaginationRequest pagination,
        CancellationToken ct)
    {
        var query = new GetInventoryReservationQuery(filter, pagination);

        var result = await sender.Send(query, ct);

        return Results.Ok(result);
    }
}
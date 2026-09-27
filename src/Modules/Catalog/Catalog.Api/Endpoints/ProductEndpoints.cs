using Carter;
using Catalog.Application.Features.Product.Commands;
using Catalog.Application.Features.Product.Dtos;
using Catalog.Application.Features.Product.Queries;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;


namespace Catalog.Api.Endpoints;

public sealed class ProductEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var adminGroup = app.MapGroup("/admin/products")
            .WithTags("Admin - Catalog")
            .RequireAuthorization("Admin");

        adminGroup.MapGet("", GetProductsAsync);

        adminGroup.MapGet("/{productId:guid}", GetProductByIdAsync);

        adminGroup.MapPost("", CreateProductAsync)
            .DisableAntiforgery();

        adminGroup.MapPut("/{productId:guid}", UpdateProductAsync)
            .DisableAntiforgery();

        adminGroup.MapDelete("/{productId:guid}", DeleteProductAsync);

        adminGroup.MapPatch("/{productId:guid}/status", ChangeProductStatusAsync);

        var group = app.MapGroup("/products")
            .WithTags("Catalog");

        group.MapGet("", GetPublicProductsAsync);

        group.MapGet("/{productId:guid}", GetPublicProductByIdAsync);

        group.MapGet("/slug/{slug}", GetPublicProductBySlugAsync);
    }

    private async Task<IResult> GetProductsAsync(
        ISender sender,
        [AsParameters] ProductFilterRequest filter,
        [AsParameters] PaginationRequest pagination,
        [AsParameters] ProductSortRequest sort,
        CancellationToken ct)
    {

        var query = new GetProductsQuery(filter, sort, pagination);

        var result = await sender.Send(query, ct);

        return Results.Ok(result);
    }

    private async Task<IResult> GetProductByIdAsync(
       ISender sender,
       [FromRoute] Guid productId,
       CancellationToken ct)
    {
        var query = new GetProductByIdQuery(productId);

        var result = await sender.Send(query, ct);

        return Results.Ok(result);
    }

    private async Task<IResult> CreateProductAsync(
        ISender sender,
        [FromBody] CreateProductDto dto,
        CancellationToken ct)
    {
        var command = new CreateProductCommand(dto);

        var result = await sender.Send(command, ct);

        return Results.Created($"/admin/products/{result}", result);
    }

    private async Task<IResult> UpdateProductAsync(
        ISender sender,
        [FromRoute] Guid productId,
        [FromBody] UpdateProductDto dto,
        CancellationToken ct)
    {
        var command = new UpdateProductCommand(productId, dto);

        await sender.Send(command, ct);

        return Results.NoContent();
    }

    private async Task<IResult> DeleteProductAsync(
        ISender sender,
        [FromRoute] Guid productId,
        CancellationToken ct)
    {
        var command = new DeleteProductCommand(productId);

        await sender.Send(command, ct);

        return Results.NoContent();
    }

    private async Task<IResult> ChangeProductStatusAsync(
        ISender sender,
        [FromRoute] Guid productId,
        [FromBody] ChangeProductStatusDto dto,
        CancellationToken ct)
    {
        var command = new ChangeProductStatusCommand(productId, dto);

        await sender.Send(command, ct);

        return Results.NoContent();
    }

    private async Task<IResult> GetPublicProductsAsync(
        ISender sender,
        [AsParameters] ProductFilterRequest filter,
        [AsParameters] PaginationRequest pagination,
        [AsParameters] ProductSortRequest sort,
        CancellationToken ct)
    {
        var query = new GetPublicProductsQuery(filter, sort, pagination);

        var result = await sender.Send(query, ct);

        return Results.Ok(result);
    }

    private async Task<IResult> GetPublicProductByIdAsync(
       ISender sender,
       [FromRoute] Guid productId,
       CancellationToken ct)
    {
        var query = new GetPublicProductByIdQuery(productId);

        var result = await sender.Send(query, ct);

        return Results.Ok(result);
    }

    private async Task<IResult> GetPublicProductBySlugAsync(
       ISender sender,
       [FromRoute] string slug,
       CancellationToken ct)
    {
        var query = new GetPublicProductBySlugQuery(slug);

        var result = await sender.Send(query, ct);

        return Results.Ok(result);
    }
}
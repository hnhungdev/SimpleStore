using Carter;
using Catalog.Application.Features.Category.Commands;
using Catalog.Application.Features.Category.Dtos;
using Catalog.Application.Features.Category.Queries;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;


namespace Catalog.Api.Endpoints;

public sealed class CategoryEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var groupAdmin = app.MapGroup("/admin/categories")
            .WithTags("Admin - Catalog")
            .RequireAuthorization("Admin");

        groupAdmin.MapGet("/", GetCategoriesAsync);

        groupAdmin.MapGet("/all", GetAllCategoriesAsync);

        groupAdmin.MapGet("/{categoryId:guid}", GetCategoryByIdAsync);

        groupAdmin.MapPost("/", CreateCategoryAsync);

        groupAdmin.MapPut("/{categoryId:guid}", UpdateCategoryAsync);

        groupAdmin.MapDelete("/{categoryId:guid}", DeleteCategoryAsync);

        var group = app.MapGroup("/categories")
            .WithTags("Catalog");

        group.MapGet("/", GetAllCategoriesAsync);

        group.MapGet("/tree", GetCategoryTreeAsync);
    }

    private static async Task<IResult> GetCategoriesAsync(
        ISender sender,
        [AsParameters] PaginationRequest pagination,
        CancellationToken ct)
    {
        var query = new GetCategoriesQuery(pagination);

        var result = await sender.Send(query, ct);

        return Results.Ok(result);
    }

    private static async Task<IResult> GetCategoryByIdAsync(
        ISender sender,
        [FromRoute] Guid categoryId,
        CancellationToken ct)
    {
        var query = new GetCategoryByIdQuery(categoryId);
        var result = await sender.Send(query, ct);

        return Results.Ok(result);
    }

    private static async Task<IResult> CreateCategoryAsync(
        ISender sender,
        [FromBody] CreateCategoryDto dto,
        CancellationToken ct)
    {
        var command = new CreateCategoryCommand(dto);
        var result = await sender.Send(command, ct);

        return Results.Created($"/categories/{result}", result);
    }

    private static async Task<IResult> UpdateCategoryAsync(
        ISender sender,
        [FromRoute] Guid categoryId,
        [FromBody] UpdateCategoryDto dto,
        CancellationToken ct)
    {
        var command = new UpdateCategoryCommand(categoryId, dto);
        await sender.Send(command, ct);

        return Results.NoContent();
    }

    private static async Task<IResult> DeleteCategoryAsync(
        ISender sender,
        [FromRoute] Guid categoryId,
        CancellationToken ct)
    {
        var command = new DeleteCategoryCommand(categoryId);
        await sender.Send(command, ct);

        return Results.NoContent();
    }

    private async Task<IResult> GetAllCategoriesAsync(
        ISender sender,
        CancellationToken ct)
    {
        var query = new GetAllCategoriesQuery();
        var result = await sender.Send(query, ct);

        return Results.Ok(result);
    }

    private async Task<IResult> GetCategoryTreeAsync(
       ISender sender,
       CancellationToken ct)
    {
        var query = new GetCategoryTreeQuery();

        var result = await sender.Send(query, ct);

        return Results.Ok(result);
    }
}
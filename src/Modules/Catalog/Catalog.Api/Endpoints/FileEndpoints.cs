using Carter;
using Catalog.Api.Extensions;
using Catalog.Application.Features.File.Commands;
using Catalog.Application.Features.File.Dtos;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;


namespace Catalog.Api.Endpoints;

public sealed class FileEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/admin/files")
            .WithTags("Admin - Catalog")
            .RequireAuthorization("Admin");

        group.MapPost("/upload", UploadImageAsync)
            .DisableAntiforgery();

        group.MapDelete("/", DeleteImageAsync);
    }

    private static async Task<IResult> UploadImageAsync(
        ISender sender,
        IFormFile file,
        CancellationToken ct)
    {
        var error = FileUploadValidator.ValidateImageFile(file);

        if (error is not null)
            throw new ClientValidationException(error);

        await using var stream = file.OpenReadStream();

        var command = new UploadImageCommand
        {
            Group = "products",
            FileStream = stream,
            FileName = file.FileName,
            ContentType = file.ContentType,
            FileSize = file.Length
        };

        var result = await sender.Send(command, ct);

        return Results.Ok(result);
    }

    private static async Task<IResult> DeleteImageAsync(
        ISender sender,
        [FromBody] DeleteFileRequest request,
        CancellationToken ct)
    {
        await sender.Send(new DeleteImageCommand(request), ct);

        return Results.NoContent();
    }
}
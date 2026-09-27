namespace Catalog.Application.Features.File.Dtos;

public record UploadImageResponse(
    string ObjectKey,
    string Url);
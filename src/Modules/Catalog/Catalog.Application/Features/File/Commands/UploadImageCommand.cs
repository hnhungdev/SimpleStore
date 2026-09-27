using Catalog.Application.Features.File.Dtos;
using Catalog.Application.Abstractions;

namespace Catalog.Application.Features.File.Commands;

public class UploadImageCommand : ICommand<UploadImageResponse>
{
    public required string Group { get; set; }
    public required Stream FileStream { get; set; }
    public required string FileName { get; set; }
    public required string ContentType { get; set; }
    public long FileSize { get; set; }
}

public class UploadImageCommandHandler(IFileStorageService fileStorage) : ICommandHandler<UploadImageCommand, UploadImageResponse>
{
    public async Task<UploadImageResponse> Handle(UploadImageCommand command, CancellationToken ct)
    {
        // 1. Build unique object key
        var extension = Path.GetExtension(command.FileName);
        var objectKey = $"{command.Group}/{Guid.NewGuid()}{extension}";

        // 2. Upload file
        var uploaded = await fileStorage.UploadAsync(new UploadFileRequest
        {
            Stream = command.FileStream,
            ObjectKey = objectKey,
            ContentType = command.ContentType,
            Size = command.FileSize,
            IsPublic = true
        }, ct);

        return new UploadImageResponse(objectKey, uploaded.Url);
    }
}

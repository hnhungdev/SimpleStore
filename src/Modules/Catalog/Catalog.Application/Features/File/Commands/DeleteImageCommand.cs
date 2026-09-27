using Catalog.Application.Features.File.Dtos;
using Catalog.Application.Abstractions;

namespace Catalog.Application.Features.File.Commands;

public sealed record DeleteImageCommand(DeleteFileRequest request) : ICommand;

public class DeleteImageCommandHandler(IFileStorageService fileStorage) : ICommandHandler<DeleteImageCommand>
{
    public async Task Handle(DeleteImageCommand command, CancellationToken ct)
    {
        await fileStorage.DeleteAsync(command.request.ObjectKey, ct);
    }
}
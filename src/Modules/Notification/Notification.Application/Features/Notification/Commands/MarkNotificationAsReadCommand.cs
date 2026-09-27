using Notification.Application.Abstractions;
using Notification.Domain.Constants;

namespace Notification.Application.Features.Notification.Commands;

public sealed record MarkNotificationAsReadCommand(Guid UserId, Guid Id) : ICommand;

public sealed class MarkNotificationAsReadCommandValidator
    : AbstractValidator<MarkNotificationAsReadCommand>
{
    public MarkNotificationAsReadCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .WithMessage(NotificationErrorCode.BadRequest);
    }
}

public class MarkNotificationAsReadCommandHandler(IUnitOfWork unitOfWork)
    : ICommandHandler<MarkNotificationAsReadCommand>
{
    public async Task Handle(MarkNotificationAsReadCommand command, CancellationToken ct)
    {
        var notification = await unitOfWork.UserNotifications
            .GetNotificationByIdAsync(command.Id, command.UserId, ct)
            ?? throw new NotFoundException(NotificationErrorCode.NotificationNotFound);

        notification.MarkAsRead();

        await unitOfWork.SaveChangesAsync(ct);
    }
}

using Notification.Application.Abstractions;

namespace Notification.Application.Features.Notification.Commands;

public sealed record MarkAllNotificationAsReadCommand(Guid UserId) : ICommand;

public sealed class MarkAllNotificationAsReadCommandValidator
    : AbstractValidator<MarkNotificationAsReadCommand>
{
    public MarkAllNotificationAsReadCommandValidator()
    {

    }
}

public class MarkAllNotificationAsReadCommandHandler(IUnitOfWork unitOfWork)
    : ICommandHandler<MarkAllNotificationAsReadCommand>
{
    public async Task Handle(MarkAllNotificationAsReadCommand command,
        CancellationToken ct)
    {
        var notifications = await unitOfWork.UserNotifications
            .GetAllNotificationsUnreadAsync(command.UserId, ct);

        foreach (var notification in notifications)
        {
            notification.MarkAsRead();
        }

        await unitOfWork.SaveChangesAsync(ct);
    }
}

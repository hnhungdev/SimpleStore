using Notification.Application.Abstractions;
using Notification.Application.Models;
using Notification.Application.Strategy;
using Notification.Domain.Entities;
using Notification.Domain.Enums;

namespace Notification.Infrastructure.Senders;

public sealed class InAppNotificationSender(IUnitOfWork unitOfWork) : INotificationSender
{
    public ChannelType Channel => ChannelType.InApp;

    public async Task<ChannelResult> SendAsync(NotificationContext context, CancellationToken ct)
    {
        try
        {

            var notification = UserNotificationEntity.Create(
                userId: context.UserId,
                title: context.Subject,
                message: context.Body!,
                notificationType: NotificationType.OrderCreated,
                targetUrl: context.TargetUrl);

            await unitOfWork.UserNotifications.AddAsync(notification, ct);


            await unitOfWork.SaveChangesAsync(ct);

            return ChannelResult.Success();
        }
        catch (Exception ex)
        {
            return ChannelResult.Failure(ex.Message);
        }
    }
}

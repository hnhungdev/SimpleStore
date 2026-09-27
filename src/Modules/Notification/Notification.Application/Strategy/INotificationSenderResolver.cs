using Notification.Domain.Enums;

namespace Notification.Application.Strategy;

public interface INotificationSenderResolver
{
    INotificationSender Resolve(ChannelType channel);
}

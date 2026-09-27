using Notification.Application.Strategy;
using Notification.Domain.Enums;

namespace Notification.Infrastructure.Resolvers;

public sealed class NotificationChannelResolver : INotificationSenderResolver
{
    private readonly IReadOnlyDictionary<ChannelType, INotificationSender> _map;

    public NotificationChannelResolver(IEnumerable<INotificationSender> channels)
        => _map = channels.ToDictionary(c => c.Channel);

    public INotificationSender Resolve(ChannelType channel) =>
        _map.TryGetValue(channel, out var impl)
            ? impl
            : throw new Exception($"No channel registered for {channel}.");
}

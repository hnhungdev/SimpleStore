using Notification.Domain.Enums;

namespace Notification.Domain.Entities;

public sealed class NotificationTemplateEntity : AuditableEntity
{
    public string Key { get; private set; } = default!;

    public ChannelType Channel { get; private set; }

    public string Subject { get; private set; } = default!;

    public string Body { get; private set; } = default!;

    public bool IsHtml { get; private set; }

    public static NotificationTemplateEntity Create(
        string key,
        ChannelType channel,
        string subject,
        bool isHtml,
        string body)
    {
        return new NotificationTemplateEntity()
        {
            Key = key,
            Channel = channel,
            Subject = subject,
            Body = body,
            IsHtml = isHtml,
        };
    }
}

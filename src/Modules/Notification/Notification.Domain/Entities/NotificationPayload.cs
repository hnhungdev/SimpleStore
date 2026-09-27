using Notification.Domain.Constants;
using Notification.Domain.Enums;

namespace Notification.Domain.Entities;

public sealed class NotificationPayload
{
    public ChannelType Channel { get; private set; }

    public Guid UserId { get; private set; }

    public string Recipient { get; private set; } = default!;

    public string Subject { get; private set; } = default!;

    public string Body { get; private set; } = default!;

    public bool IsHtml { get; private set; }

    public string? TargetUrl { get; private set; }

    public static NotificationPayload Create(
        ChannelType channel,
        Guid userId,
        string recipient,
        string subject,
        string body,
        bool isHtml = false,
        string? targetUrl = null)
    {
        if (string.IsNullOrWhiteSpace(recipient))
            throw new DomainException(NotificationErrorCode.RecipientIsRequired);

        if (string.IsNullOrWhiteSpace(subject))
            throw new DomainException(NotificationErrorCode.SubjectIsRequired);

        if (string.IsNullOrWhiteSpace(body))
            throw new DomainException(NotificationErrorCode.BodyIsRequired);

        return new NotificationPayload
        {
            Channel = channel,
            UserId = userId,
            Recipient = recipient,
            Subject = subject,
            Body = body,
            IsHtml = isHtml,
            TargetUrl = targetUrl
        };
    }
}
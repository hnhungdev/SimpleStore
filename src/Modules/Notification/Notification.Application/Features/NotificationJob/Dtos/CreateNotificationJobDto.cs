using Notification.Domain.Enums;

namespace Notification.Application.Features.NotificationJobs.Dtos;

public sealed class CreateNotificationJobDto
{
    public Guid EventId { get; set; }

    public Guid UserId { get; set; }

    public string TemplateKey { get; set; } = string.Empty;

    public ChannelType ChannelType { get; set; }

    public string Recipient { get; set; } = string.Empty;

    public Dictionary<string, object>? TemplateVariables { get; set; }

    public NotificationJobPriority Priority { get; set; } = NotificationJobPriority.Medium;

    public int MaxAttempts { get; set; } = 3;

    public string? TargetUrl { get; set; }
}


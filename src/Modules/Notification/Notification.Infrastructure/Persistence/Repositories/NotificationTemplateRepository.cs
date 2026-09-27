using Microsoft.EntityFrameworkCore;
using Notification.Domain.Repositories;
using Notification.Domain.Entities;
using Notification.Domain.Enums;
using Notification.Infrastructure.Persistence.Configurations;

namespace Notification.Infrastructure.Persistence.Repositories;

public class NotificationTemplateRepository : INotificationTemplateRepository
{
    private readonly NotificationDbContext context;

    public NotificationTemplateRepository(NotificationDbContext context)
    {
        this.context = context;
    }

   
    public Task<NotificationTemplateEntity?> GetTemplateAsync(string key, ChannelType channel, CancellationToken ct = default)
    {
        return context.NotificationTemplates
            .FirstOrDefaultAsync(x => x.Key == key && x.Channel == channel, ct);
    }

}
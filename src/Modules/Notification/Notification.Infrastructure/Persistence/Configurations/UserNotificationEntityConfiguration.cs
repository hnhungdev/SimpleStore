using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notification.Domain.Entities;

namespace Notification.Infrastructure.Persistence.Configurations;

public sealed class UserNotificationEntityConfiguration
    : IEntityTypeConfiguration<UserNotificationEntity>
{
    public void Configure(EntityTypeBuilder<UserNotificationEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Title)
            .HasMaxLength(255);

        builder.Property(x => x.NotificationType)
            .HasConversion<int>();

        builder.Property(x => x.IsRead)
            .HasDefaultValue(false);

        builder.Property(x => x.TargetUrl)
            .HasMaxLength(255);

        builder.Property(x => x.CreatedBy)
            .HasMaxLength(50);

        builder.Property(x => x.UpdatedBy)
            .HasMaxLength(50);

        builder.HasIndex(x => x.UserId);

        builder.HasIndex(x => new { x.UserId, x.IsRead });

        builder.HasIndex(x => x.CreatedAt);
    }
}
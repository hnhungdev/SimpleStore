using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notification.Domain.Entities;

namespace Notification.Infrastructure.Persistence.Configurations;

public sealed class NotificationJobEntityConfiguration 
    : IEntityTypeConfiguration<NotificationJobEntity>
{
    public void Configure(EntityTypeBuilder<NotificationJobEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Status)
            .HasConversion<int>();

        builder.Property(x => x.Priority)
            .HasConversion<int>();

        builder.Property(x => x.LastErrorMessage)
            .HasMaxLength(1000);

        builder.Property(x => x.AttemptCount)
            .HasDefaultValue(0);

        builder.Property(x => x.MaxAttempts)
            .HasDefaultValue(3);

        builder.ComplexProperty(x => x.Payload, payloadBuilder =>
        {
            payloadBuilder.Property(p => p.Channel)
                .HasConversion<int>();

            payloadBuilder.Property(p => p.Recipient)
                .HasMaxLength(255);

            payloadBuilder.Property(p => p.Subject)
                .HasMaxLength(255);

            payloadBuilder.Property(p => p.TargetUrl)
                .HasMaxLength(500);
        });

        builder.Property(x => x.CreatedBy)
            .HasMaxLength(50);

        builder.Property(x => x.UpdatedBy)
            .HasMaxLength(50);

        builder.HasIndex(x => new
        {
            x.Status,
            x.ClaimedAt,
            x.NextAttemptAt
        });

        builder.HasIndex(x => new
        {
            x.Status,
            x.Priority
        });
    }
}
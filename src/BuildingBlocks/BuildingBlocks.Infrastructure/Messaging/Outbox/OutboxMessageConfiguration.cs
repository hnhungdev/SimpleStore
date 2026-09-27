using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BuildingBlocks.Infrastructure.Messaging.Outbox;

public sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.EventType)
            .HasMaxLength(256);

        builder.Property(x => x.LastErrorMessage)
            .HasMaxLength(4000);

        builder.Property(x => x.AttemptCount)
            .HasDefaultValue(0);

        builder.Property(x => x.MaxAttempts)
            .HasDefaultValue(3);

        builder.HasIndex(x => new
        {
            x.ProcessedAt,
            x.NextAttemptAt,
            x.OccurredAt
        })
        .HasFilter("[processed_at] IS NULL");
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BuildingBlocks.Infrastructure.Messaging.Inbox;

public sealed class InboxMessageConfiguration : IEntityTypeConfiguration<InboxMessage>
{
    public void Configure(EntityTypeBuilder<InboxMessage> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .ValueGeneratedNever();

        builder.Property(m => m.EventType)
            .HasMaxLength(256);

        builder.Property(m => m.LastErrorMessage)
            .HasMaxLength(4000);

        builder.HasIndex(m => m.ProcessedAt)
            .HasFilter("[processed_at] IS NULL");
    }
}

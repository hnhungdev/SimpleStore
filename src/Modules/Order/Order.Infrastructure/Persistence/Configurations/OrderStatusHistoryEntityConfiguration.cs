using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Domain.Entities;

namespace Order.Infrastructure.Persistence.Configurations;

public sealed class OrderStatusHistoryEntityConfiguration : IEntityTypeConfiguration<OrderStatusHistoryEntity>
{
    public void Configure(EntityTypeBuilder<OrderStatusHistoryEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.FromStatus)
            .HasConversion<int>();

        builder.Property(x => x.ToStatus)
            .HasConversion<int>();

        builder.Property(x => x.Reason)
            .HasMaxLength(500);

        builder.Ignore(x => x.UpdatedAt);

        builder.Ignore(x => x.UpdatedBy);

        builder.HasIndex(x => x.OrderId);
    }
}

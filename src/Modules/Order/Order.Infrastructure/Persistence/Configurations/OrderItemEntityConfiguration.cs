using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Domain.Entities;

namespace Order.Infrastructure.Persistence.Configurations;

public sealed class OrderItemEntityConfiguration : IEntityTypeConfiguration<OrderItemEntity>
{
    public void Configure(EntityTypeBuilder<OrderItemEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Ignore(x => x.LineTotal);

        builder.ComplexProperty(oi => oi.Product, b =>
        {
            b.Property(p => p.Id);
            
            b.Property(p => p.Name)
                .HasMaxLength(255);

            b.Property(p => p.ThumbnailUrl)
                .HasMaxLength(500);

            b.Property(p => p.Price)
                .HasPrecision(18, 2);
        });

        builder.HasIndex(x => x.OrderId);
    }
}
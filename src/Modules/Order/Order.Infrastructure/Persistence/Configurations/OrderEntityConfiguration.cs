using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Domain.Entities;
using Order.Domain.ValueObjects;

namespace Order.Infrastructure.Persistence.Configurations;

public sealed class OrderEntityConfiguration : IEntityTypeConfiguration<OrderEntity>
{
    public void Configure(EntityTypeBuilder<OrderEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.OrderStatus)
            .HasConversion<int>();

        builder.Property(x => x.PaymentMethod)
            .HasConversion<int>();

        builder.Property(x => x.OrderNo)
            .HasConversion(
                orderNo => orderNo.Value,
                value => OrderNo.Of(value))
            .HasMaxLength(100);

        builder.HasIndex(x => x.OrderNo)
            .IsUnique();

        builder.ComplexProperty(o => o.Customer, b =>
        {
            b.Property(c => c.Id);
            
            b.Property(c => c.PhoneNumber)
                .HasMaxLength(50);

            b.Property(c => c.Name)
                .HasMaxLength(255);

            b.Property(c => c.Email)
                .HasMaxLength(255);

            b.Property(c => c.Address)
                .HasMaxLength(255);
        });


        builder.Property(x => x.TotalPrice)
            .HasPrecision(18, 2);

        builder.Property(x => x.Notes)
            .HasMaxLength(500);

        builder.Property(x => x.OrderDate)
            .HasConversion<DateOnly>();

        builder.Property(x => x.CreatedBy)
            .HasMaxLength(50);

        builder.Property(x => x.UpdatedBy)
            .HasMaxLength(50);

        builder.HasMany(o => o.OrderItems)
            .WithOne()
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Navigation(o => o.OrderItems)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(o => o.StatusHistories)
            .WithOne()
            .HasForeignKey(oh => oh.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Navigation(o => o.StatusHistories)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasIndex(x => new
        {
            x.OrderStatus,
            x.CreatedAt
        });
    }
}
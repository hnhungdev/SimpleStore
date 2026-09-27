using Cart.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cart.Infrastructure.Persistence.Configurations;

public sealed class CartItemConfiguration : IEntityTypeConfiguration<CartItemEntity>
{
    public void Configure(EntityTypeBuilder<CartItemEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

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

        builder.Ignore(x => x.LineTotal);

        builder.HasIndex(x => x.CartId);
    }
}
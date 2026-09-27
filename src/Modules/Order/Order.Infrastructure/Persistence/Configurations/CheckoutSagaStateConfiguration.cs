using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Application.Sagas;

namespace Order.Infrastructure.Persistence.Configurations;

public sealed class CheckoutSagaStateConfiguration : IEntityTypeConfiguration<CheckoutSagaState>
{
    public void Configure(EntityTypeBuilder<CheckoutSagaState> builder)
    {
        builder.HasKey(x => x.CorrelationId);

        builder.Property(x => x.CurrentState)
            .HasMaxLength(64);

        builder.Property(x => x.PaymentMethod)
            .HasMaxLength(50);

        builder.Property(x => x.FailureReason)
            .HasMaxLength(500);

        builder.Property(x => x.TotalAmount)
            .HasPrecision(18, 2);

        builder.Property(x => x.RowVersion)
            .IsRowVersion();

        builder.HasIndex(x => x.CurrentState);
        builder.HasIndex(x => x.OrderId);
    }
}
using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Persistence.Configurations;

public sealed class InventoryTransactionEntityConfiguration : IEntityTypeConfiguration<InventoryTransactionEntity>
{
    public void Configure(EntityTypeBuilder<InventoryTransactionEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.TransactionType)
            .HasConversion<int>();

        builder.HasIndex(x => x.InventoryItemId);
    }
}
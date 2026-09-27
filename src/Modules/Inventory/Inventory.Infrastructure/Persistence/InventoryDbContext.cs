using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Inventory.Domain.Entities;

namespace Inventory.Infrastructure.Persistence;

public sealed class InventoryDbContext : DbContext
{
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options)
        : base(options) { }

    public DbSet<InventoryItemEntity> InventoryItems => Set<InventoryItemEntity>();

    public DbSet<InventoryReservationEntity> InventoryReservations => Set<InventoryReservationEntity>();

    public DbSet<InventoryTransactionEntity> InventoryTransactions => Set<InventoryTransactionEntity>();
    
    public DbSet<InboxMessage> InboxMessages => Set<InboxMessage>();

    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("inventory");
        
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        builder.ApplyConfiguration(new InboxMessageConfiguration());
        builder.ApplyConfiguration(new OutboxMessageConfiguration());

        builder.UseSnakeCaseNaming();

        base.OnModelCreating(builder);
    }
}

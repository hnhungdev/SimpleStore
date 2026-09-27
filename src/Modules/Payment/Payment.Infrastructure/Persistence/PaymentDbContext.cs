using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Payment.Domain.Entities;

namespace Payment.Infrastructure.Persistence;

public sealed class PaymentDbContext : DbContext
{
    public PaymentDbContext(DbContextOptions<PaymentDbContext> options)
        : base(options) { }

    public DbSet<PaymentEntity> Payments => Set<PaymentEntity>();

    public DbSet<InboxMessage> InboxMessages => Set<InboxMessage>();

    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("payment");
        
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        builder.ApplyConfiguration(new InboxMessageConfiguration());
        builder.ApplyConfiguration(new OutboxMessageConfiguration());

        builder.UseSnakeCaseNaming();

        base.OnModelCreating(builder);
    }
}

using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Cart.Domain.Entities;

namespace Cart.Infrastructure.Persistence;

public sealed class CartDbContext : DbContext
{
    public CartDbContext(DbContextOptions<CartDbContext> options)
        : base(options) { }

    public DbSet<CartEntity> Carts => Set<CartEntity>();
    public DbSet<CartItemEntity> CartItems => Set<CartItemEntity>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("cart");
        
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        builder.UseSnakeCaseNaming();

        base.OnModelCreating(builder);
    }
}

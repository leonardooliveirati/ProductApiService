using Microsoft.EntityFrameworkCore;
using Product.Domain.Entities;

namespace Product.Infrastructure.Data;

public class ProductContext : DbContext
{
    public ProductContext(DbContextOptions<ProductContext> options) : base(options) { }

    public DbSet<ProductEntity> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<ProductEntity>(entity =>
        {
            entity.HasKey(b => b.Id);
            entity.Property(b => b.Id).ValueGeneratedOnAdd();
            entity.Property(b => b.Name).HasColumnType("nvarchar(100)");
            entity.Property(b => b.Price).HasColumnType("decimal(18,2)");
            entity.Property(b => b.Description).HasColumnType("nvarchar(250)");
        });
    }
}

using clase7_controller.Models;
using Microsoft.EntityFrameworkCore;

namespace clase7_controller.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options)
{
    public DbSet<Product> Products { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(product => product.Name).IsRequired().HasMaxLength(100);
            entity.Property(product => product.Brand).IsRequired().HasMaxLength(100);
            entity.ToTable(table =>
                table.HasCheckConstraint("CK_Products_Price_GreaterThanZero", "Price > 0")
            );
        });
    }
}

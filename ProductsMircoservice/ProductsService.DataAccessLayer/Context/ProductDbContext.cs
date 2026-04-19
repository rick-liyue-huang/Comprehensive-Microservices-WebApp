using Microsoft.EntityFrameworkCore;
using ProductsService.DataAccessLayer.Entities;

namespace ProductsService.DataAccessLayer.Context;

public class ProductDbContext : DbContext
{
    public ProductDbContext(DbContextOptions<ProductDbContext> options) : base( options)
    { }
    
    public DbSet<Product> Products { get; set; }
    
    override protected void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Product>().ToTable("products");
    }
}
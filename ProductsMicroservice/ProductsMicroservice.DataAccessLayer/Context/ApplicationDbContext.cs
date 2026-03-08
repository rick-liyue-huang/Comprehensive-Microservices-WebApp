using Microsoft.EntityFrameworkCore;
using ProductsMicroservice.DataAccessLayer.Entities;

namespace ProductsMicroservice.DataAccessLayer.Context;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // modelBuilder.Entity<Product>().ToTable("Products");
    }
}
using Microsoft.EntityFrameworkCore;
using productsService.DataAccessLayer.Entities;

namespace productsService.DataAccessLayer.Context;

public class ApplicationDbContext : DbContext
{

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
        
    }
    
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
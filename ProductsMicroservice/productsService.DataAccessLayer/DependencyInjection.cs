using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using productsService.DataAccessLayer.Context;

namespace productsService.DataAccessLayer;

public static class DependencyInjection
{
    
    public static IServiceCollection AddDataAccessLayer(
        this IServiceCollection services, IConfiguration configuration)
    {
        // Add Data Access Layer

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseMySQL(configuration.GetConnectionString("DefaultConnection")!);
        });
        
        return services;
    }
}
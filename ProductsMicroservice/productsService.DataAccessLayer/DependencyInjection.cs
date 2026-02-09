using Microsoft.Extensions.DependencyInjection;

namespace productsService.DataAccessLayer;

public static class DependencyInjection
{
    
    public static IServiceCollection AddDataAccessLayer(this IServiceCollection services)
    {
        // Add Data Access Layer
        return services;
    }
}
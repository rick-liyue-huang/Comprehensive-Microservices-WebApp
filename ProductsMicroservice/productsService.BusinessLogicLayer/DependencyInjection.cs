using Microsoft.Extensions.DependencyInjection;
using productsService.BusinessLogicLayer.Mappers;

namespace productsService.BusinessLogicLayer;

public static class DependencyInjection
{
    
    public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services)
    {
        // Add Business Logic Layer
        services.AddAutoMapper(cfg =>
        {}, typeof(ProductAddRequestToProductMappingProfile).Assembly);
        
        return services;
    }
}
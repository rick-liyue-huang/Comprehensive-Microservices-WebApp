using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using productsService.BusinessLogicLayer.Mappers;
using productsService.BusinessLogicLayer.ServiceContracts;
using productsService.BusinessLogicLayer.Services;
using productsService.BusinessLogicLayer.Validators;

namespace productsService.BusinessLogicLayer;

public static class DependencyInjection
{
    
    public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services)
    {
        // Add Business Logic Layer
        services.AddAutoMapper(cfg =>
        {}, typeof(ProductAddRequestToProductMappingProfile).Assembly);

        services.AddValidatorsFromAssemblyContaining<ProductAddRequestValidator>();
        services.AddScoped<IProductService, ProductService>();
        
        return services;
    }
}
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductsService.BusinessLogicLayer.Mappers;
using ProductsService.BusinessLogicLayer.ServiceContracts;
using ProductsService.BusinessLogicLayer.Validators;

namespace ProductsService.BusinessLogicLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services, IConfiguration configuration)
    {
        
        services.AddAutoMapper(cfg =>
        {
            cfg.LicenseKey = configuration["AutoMapper:licenseKey"];
        }, typeof(ProductAddRequestToProductMappingProfile).Assembly);

        services.AddValidatorsFromAssemblyContaining<ProductAddRequestValidator>();
        services.AddScoped<IProductsService, Services.ProductsService>();
        return services;
    }
}
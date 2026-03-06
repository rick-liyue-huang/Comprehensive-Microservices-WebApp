using Microsoft.Extensions.DependencyInjection;
using UsersMicroservice.Core.ServiceContracts;
using UsersMicroservice.Core.Services;
using Microsoft.Extensions.Configuration;

namespace UsersMicroservice.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
    {
        string autoMapperLicenseKey = configuration["AutoMapper:LicenseKey"]!;
        services.AddAutoMapper(cfg => 
        { 
            cfg.LicenseKey = autoMapperLicenseKey;
        }, typeof(DependencyInjection).Assembly);
        services.AddScoped<IUsersService, UsersService>();
        
        return services;
    }
}
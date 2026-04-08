using Microsoft.Extensions.DependencyInjection;
using UsersMicroservice.Core.ServiceContracts;
using UsersMicroservice.Core.Services;

namespace UsersMicroservice.Core;

public static class DependencyInjection
{
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddScoped<IUsersService, UsersService>();
            
            return services;
        }
}
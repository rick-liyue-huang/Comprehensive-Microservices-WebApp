using Microsoft.Extensions.DependencyInjection;
using usersService.Core.ServiceContracts;
using usersService.Core.Services;

namespace usersService.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCoreService(this IServiceCollection services)
    {
        services.AddScoped<IUsersService, UsersService>();
        return services;
    }
}
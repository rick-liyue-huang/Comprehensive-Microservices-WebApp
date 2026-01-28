using Microsoft.Extensions.DependencyInjection;

namespace usersService.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCoreService(this IServiceCollection services)
    {
        return services;
    }
}
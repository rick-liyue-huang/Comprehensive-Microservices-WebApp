using Microsoft.Extensions.DependencyInjection;
using usersService.Core.RepositoryContracts;
using usersService.Infrastructure.Repositories;

namespace usersService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureService(this IServiceCollection services)
    {
        services.AddScoped<IUsersRepository, UsersRepository>();
        
        return services;
    }
}
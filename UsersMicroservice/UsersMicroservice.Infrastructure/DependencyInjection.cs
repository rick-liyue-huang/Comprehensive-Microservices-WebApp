using Microsoft.Extensions.DependencyInjection;
using UsersMicroservice.Core.RepositoryContracts;
using UsersMicroservice.Infrastructure.Repositories;

namespace UsersMicroservice.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IUsersRepository, UsersRepository>();
        
        return services;
    }
}
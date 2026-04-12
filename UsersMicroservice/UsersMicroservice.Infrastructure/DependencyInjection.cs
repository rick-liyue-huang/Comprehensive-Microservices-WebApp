using Microsoft.Extensions.DependencyInjection;
using UsersMicroservice.Core.RepositoryContracts;
using UsersMicroservice.Infrastructure.Repositories;
using UsersMicroservice.Infrastructure.DbContext;

namespace UsersMicroservice.Infrastructure;

public static class DependencyInjection
{
    /// <summary>
    /// Add Infrastructure services to the IoC container
    /// </summary>
    /// <param name="services"></param>
    /// <returns>
    /// services
    /// </returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // TODO: Add services to the IoC container 
        // Infrastructure services often include data access, caching , and other lower-layer services.

        services.AddScoped<IUsersRepository, UsersRepository>();
        services.AddScoped<DapperDbContext>();

        return services;
    }
}
using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using UsersMicroservice.Core.RepositoryContracts;
using UsersMicroservice.Infrastructure.DbContext;
using UsersMicroservice.Infrastructure.Repositories;

namespace UsersMicroservice.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IDbConnection>(sp =>
            new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection")));
        
        services.AddScoped<IUsersRepository, UsersRepository>();
        services.AddScoped<DapperDbContext>();
        
        return services;
    }
}
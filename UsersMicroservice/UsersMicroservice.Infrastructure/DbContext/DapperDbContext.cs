using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace UsersMicroservice.Infrastructure.DbContext;

public class DapperDbContext
{
    private readonly IConfiguration _configuration;
    private readonly IDbConnection _connection;
    public DapperDbContext(IConfiguration configuration, IDbConnection connection)
    {
        _configuration = configuration;
        _connection = connection;
        string? connectionString = _configuration.GetConnectionString("DefaultConnection");

        _connection = new NpgsqlConnection(connectionString);
    }
    
    public IDbConnection GetConnection()
    {
        return _connection;
    }
}
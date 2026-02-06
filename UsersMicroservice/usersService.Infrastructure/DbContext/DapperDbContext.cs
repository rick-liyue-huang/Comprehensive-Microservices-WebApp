using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace usersService.Infrastructure.DbContext;

public class DapperDbContext
{
    private readonly IConfiguration _configuration;
    private readonly IDbConnection _dbConnection;
    
    public DapperDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
        string? connectionString = _configuration.GetConnectionString("DefaultConnection");
        
        // Create a new NpgsqlConnection using the connection string
        _dbConnection = new NpgsqlConnection(connectionString);
    }
    
    public IDbConnection DbConnection => _dbConnection;
}
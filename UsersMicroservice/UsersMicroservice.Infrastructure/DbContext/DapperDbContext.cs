using Microsoft.Extensions.Configuration;
using Npgsql;


namespace UsersMicroservice.Infrastructure.DbContext;

public class DapperDbContext
{
  private readonly IConfiguration _configuration;
  private readonly NpgsqlConnection _connection;

  public DapperDbContext(IConfiguration configuration)
  {
    _configuration = configuration;

    string? connectionString = configuration.GetConnectionString("DefaultConnection");

    _connection = new NpgsqlConnection(connectionString);
  }

  public NpgsqlConnection DbConnection => _connection;
}

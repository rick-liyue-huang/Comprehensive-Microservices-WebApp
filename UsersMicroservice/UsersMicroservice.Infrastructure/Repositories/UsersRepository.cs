using Dapper;
using UsersMicroservice.Core.Dtos;
using UsersMicroservice.Core.RepositoryContracts;
using UsersMicroservice.Domain.Entities;
using UsersMicroservice.Infrastructure.DbContext;

namespace UsersMicroservice.Infrastructure.Repositories;

public class UsersRepository(DapperDbContext dapperDbContext) : IUsersRepository
{
  public async Task<ApplicationUser> AddUser(ApplicationUser user)
  {
    user.UserId = Guid.NewGuid();

    string query =
      "INSERT INTO Users (userid, email, password, personname, gender) VALUES (@UserId, @Email, @Password, @PersonName, @Gender)";

    int rowCountAffected = await dapperDbContext.DbConnection.ExecuteAsync(query, user);

    return rowCountAffected > 0 ? user : null;
  }

  public async Task<ApplicationUser?> GetUserByEmailAndPassword(string? email, string? password)
  {
    // return new ApplicationUser()
    // {
    //   UserId = Guid.NewGuid(),
    //   Email = email,
    //   Password = password,
    //   PersonName = "person name",
    //   Gender = nameof(GenderOptions.Male)
    // };

    string query = "SELECT * FROM users WHERE email = @Email AND password = @Password";

    var parameters = new DynamicParameters();
    parameters.Add("Email", email);
    parameters.Add("Password", password);

    ApplicationUser? user =
      await dapperDbContext.DbConnection.QueryFirstOrDefaultAsync<ApplicationUser>(query, parameters);

    return user;
  }
}

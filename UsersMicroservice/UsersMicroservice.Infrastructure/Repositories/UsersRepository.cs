using Dapper;
using UsersMicroservice.Core.Dtos;
using UsersMicroservice.Core.RepositoryContracts;
using UsersMicroservice.Domain.Entities;
using UsersMicroservice.Infrastructure.DbContext;

namespace UsersMicroservice.Infrastructure.Repositories;

public class UsersRepository(DapperDbContext dbContext) : IUsersRepository
{
    public async Task<ApplicationUser?> AddUser(ApplicationUser user)
    {
        user.UserId = Guid.NewGuid();
        
        string query = "INSERT INTO Users (UserId, Email, Password, PersonName, Gender) VALUES (@UserId, @Email, @Password, @PersonName, @Gender)";

        int rowCountAffected = await dbContext.GetConnection().ExecuteAsync(query, user);

        if (rowCountAffected > 0)
        {
            return user;
        }
        else
        {
            return null;
        }
    }

    public async Task<ApplicationUser?> GetUserByEmailAndPassword(string email, string password)
    {
        // return new ApplicationUser()
        // {
        //     Email = email,
        //     Password = password,
        //     UserId = Guid.NewGuid(),
        //     PersonName = "Test",
        //     Gender = GenderOptions.Male.ToString()
        // };
        
        string query = "SELECT * FROM Users WHERE Email = @Email AND Password = @Password";

        ApplicationUser? user = await dbContext.GetConnection().QueryFirstOrDefaultAsync<ApplicationUser>(query, new { Email = email, Password = password });
        
        if (user == null) return null;
        return user;

    }
}
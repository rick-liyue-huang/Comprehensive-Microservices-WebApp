using Dapper;
using usersService.Core.Dtos;
using usersService.Core.RepositoryContracts;
using usersService.Domain.Entities;
using usersService.Infrastructure.DbContext;

namespace usersService.Infrastructure.Repositories;

public class UsersRepository(DapperDbContext dapperDbContext) : IUsersRepository
{
    public async Task<ApplicationUser?> AddUser(ApplicationUser user)
    {
        user.UserId = Guid.NewGuid();
        
        // SQL Query to insert user into database,
        string query = "INSERT INTO public.users(\"userid\", \"email\", \"password\", \"personname\", \"gender\") " +
                       "VALUES(@UserId, @Email, @Password, @PersonName, @Gender)";

        int rowCountAffected = await dapperDbContext.DbConnection.ExecuteAsync(query, user);

        if (rowCountAffected > 0)
        {
            return user;
        }
        else
        {
            return null;
        }
    }

    public async Task<ApplicationUser?> GetUserByEmailAndPassword(string? email, string? password)
    {
        
        // SQL Query to get single user by email and password,
        string query = "SELECT * FROM public.users WHERE email = @Email AND password = @Password";
        
        ApplicationUser? user = await dapperDbContext.DbConnection.QueryFirstOrDefaultAsync<ApplicationUser>(query, new {Email = email, Password = password});
        
        // return new ApplicationUser()
        // {
        //     UserId = Guid.NewGuid(),
        //     Email = email,
        //     Password = password,
        //     PersonName = "Rick",
        //     Gender = nameof(GenderOptions.Male)
        // };
        
        if (user == null)
            return null;
        
        return user;
    }
}

// docker run --name user_postgres_db -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=password -e POSTGRES_DB=user_service_db -p 5436:5432 -d postgres
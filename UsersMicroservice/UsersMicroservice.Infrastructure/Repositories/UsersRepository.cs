using UsersMicroservice.Core.Dtos;
using UsersMicroservice.Core.RepositoryContracts;
using UsersMicroservice.Domain.Entities;

namespace UsersMicroservice.Infrastructure.Repositories;

public class UsersRepository : IUsersRepository
{
    public async Task<ApplicationUser?> AddUser(ApplicationUser user)
    {
        user.UserId = Guid.NewGuid();
        return user;
    }

    public async Task<ApplicationUser?> GetUserByEmailAndPassword(string email, string password)
    {
        return new ApplicationUser()
        {
            Email = email,
            Password = password,
            UserId = Guid.NewGuid(),
            PersonName = "Test",
            Gender = GenderOptions.Male.ToString()
        };

    }
}
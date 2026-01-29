using usersService.Core.Dtos;
using usersService.Core.RepositoryContracts;
using usersService.Domain.Entities;

namespace usersService.Infrastructure.Repositories;

public class UsersRepository : IUsersRepository
{
    public async Task<ApplicationUser?> AddUser(ApplicationUser user)
    {
        user.UserId = Guid.NewGuid();
        
        return user;
    }

    public async Task<ApplicationUser?> GetUserByEmailAndPassword(string? email, string? password)
    {
        return new ApplicationUser()
        {
            UserId = Guid.NewGuid(),
            Email = email,
            Password = password,
            PersonName = "Rick",
            Gender = nameof(GenderOptions.Male)
        };
    }
}
using usersService.Domain.Entities;

namespace usersService.Core.RepositoryContracts;

public interface IUsersRepository
{
    Task<ApplicationUser?> AddUser(ApplicationUser user);
    Task<ApplicationUser?> GetUserByEmailAndPassword(string? email, string? password);
}
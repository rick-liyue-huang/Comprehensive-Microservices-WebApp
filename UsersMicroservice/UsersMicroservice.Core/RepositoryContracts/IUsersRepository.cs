using UsersMicroservice.Domain.Entities;

namespace UsersMicroservice.Core.RepositoryContracts;

public interface IUsersRepository
{
    /// <summary>
    /// Add a new user
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    Task<ApplicationUser?> AddUser(ApplicationUser user);
    
    /// <summary>
    /// Get user by email and password
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    Task<ApplicationUser?> GetUserByEmailAndPassword(string email, string password);
}
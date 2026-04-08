using static System.Net.Mime.MediaTypeNames;
using UsersMicroservice.Domain.Entities;
namespace UsersMicroservice.Core.RepositoryContracts;

public interface IUsersRepository
{
    /// <summary>
    /// Method to add a user to the data store and return the added user
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    Task<ApplicationUser?> AddUser(ApplicationUser user);

    /// <summary>
    /// return the specific user by email and password
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    Task<ApplicationUser?> GetUserByEmailAndPassword(string? email, string? password);
}

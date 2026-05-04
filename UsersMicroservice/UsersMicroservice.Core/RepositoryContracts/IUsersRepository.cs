using UsersMicroservice.Domain.Entities;
using static System.Net.Mime.MediaTypeNames;
namespace UsersMicroservice.Core.RepositoryContracts;

public interface IUsersRepository
{
  Task<ApplicationUser?> AddUser(ApplicationUser user);
  Task<ApplicationUser?> GetUserByEmailAndPassword(string? email, string? password);
}

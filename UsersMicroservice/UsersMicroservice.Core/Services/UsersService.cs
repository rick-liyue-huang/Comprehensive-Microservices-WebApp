using UsersMicroservice.Core.Dtos;
using UsersMicroservice.Core.RepositoryContracts;
using UsersMicroservice.Core.ServiceContracts;
using UsersMicroservice.Domain.Entities;
using static System.Net.Mime.MediaTypeNames;
namespace UsersMicroservice.Core.Services;

public class UsersService(IUsersRepository usersRepository) : IUsersService
{
  public async Task<AuthenticationResponse?> Login(LoginRequest loginRequest)
  {
    ApplicationUser? user = await usersRepository.GetUserByEmailAndPassword(loginRequest.Email, loginRequest.Password);
    if (user == null) return null;

    return new AuthenticationResponse(user.UserId, user.Email, user.PersonName, user.Gender, "token", true);
  }

  public async Task<AuthenticationResponse?> Register(RegisterRequest registerRequest)
  {
    ApplicationUser user = new ApplicationUser()
    {
      Email = registerRequest.Email,
      PersonName = registerRequest.PersonName,
      Password = registerRequest.Password,
      Gender = registerRequest.Gender.ToString()
    };
    ApplicationUser? newUser = await usersRepository.AddUser(user);

    if (newUser == null) return null;

    return new AuthenticationResponse(
      newUser.UserId, 
      newUser.Email,
      newUser.PersonName,
      newUser.Gender,
      "token",
      true
    );
  }
}

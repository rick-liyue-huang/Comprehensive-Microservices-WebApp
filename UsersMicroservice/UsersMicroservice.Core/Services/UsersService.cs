using UsersMicroservice.Core.ServiceContracts;
using UsersMicroservice.Core.Dtos;
using UsersMicroservice.Core.RepositoryContracts;
using UsersMicroservice.Domain.Entities;
using static System.Net.Mime.MediaTypeNames;

namespace UsersMicroservice.Core.Services;

public class UsersService(IUsersRepository usersRepository) : IUsersService
{
	public async Task<AuthenticationResponse?> Login(LoginRequest loginRequest)
	{
		ApplicationUser? user = await usersRepository.GetUserByEmailAndPassword(loginRequest.Email, loginRequest.Password);

		if (user == null)
		{
		return null;
		}

		return new AuthenticationResponse(user.UserId, user.Email, user.PersonName, user.Gender, "token", Success: true);
	}

	public async Task<AuthenticationResponse?> Register(RegisterRequest registerRequest)
	{
		ApplicationUser user = new ApplicationUser()
		{
			PersonName = registerRequest.PersonName,
			Email = registerRequest.Eamil,
			Password = registerRequest.Password,
			Gender = registerRequest.Gender.ToString()
		};

		ApplicationUser? registeredUser = await usersRepository.AddUser(user);

		if (registeredUser == null) return null;

		return new AuthenticationResponse(registeredUser.UserId, registeredUser.Email, registeredUser.PersonName,
			registeredUser.Gender, "token", Success: true);
	}
}

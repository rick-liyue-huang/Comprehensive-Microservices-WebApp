using usersService.Core.Dtos;
using usersService.Core.RepositoryContracts;
using usersService.Core.ServiceContracts;
using usersService.Domain.Entities;

namespace usersService.Core.Services;

public class UsersService(IUsersRepository usersRepository) : IUsersService
{
    public async Task<AuthenticationResponse> Login(LoginRequest request)
    {
        ApplicationUser? user = await usersRepository.GetUserByEmailAndPassword(request.Email, request.Password);

        if (user == null)
        {
            return null;
        }
        
        // Return successful response
        return new AuthenticationResponse(
            user.UserId, 
            user.Email, 
            user.PersonName, 
            user.Gender, 
            "token", 
            true);
    }

    public async Task<AuthenticationResponse> Register(RegisterRequest request)
    {
        ApplicationUser? user = new ApplicationUser()
        {
            PersonName = request.PersonName,
            Email = request.Email,
            Password = request.Password,
            Gender = request.Gender.ToString()
        };
        
        ApplicationUser? registeredUser = await usersRepository.AddUser(user);
        
        if (registeredUser == null)
        {
            return null;
        }
        
        // Return successful response
        return new AuthenticationResponse(
            registeredUser.UserId, 
            registeredUser.Email, 
            registeredUser.PersonName, 
            registeredUser.Gender, 
            "token", 
            true);
    }
}
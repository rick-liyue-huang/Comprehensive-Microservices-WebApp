using AutoMapper;
using usersService.Core.Dtos;
using usersService.Core.Mappers;
using usersService.Core.RepositoryContracts;
using usersService.Core.ServiceContracts;
using usersService.Domain.Entities;

namespace usersService.Core.Services;

public class UsersService(IUsersRepository usersRepository, IMapper mapper) : IUsersService
{
    public async Task<AuthenticationResponse> Login(LoginRequest request)
    {
        ApplicationUser? user = await usersRepository.GetUserByEmailAndPassword(request.Email, request.Password);

        if (user == null)
        {
            return null;
        }
        
        // Return successful response
        // return new AuthenticationResponse(
        //     user.UserId, 
        //     user.Email, 
        //     user.PersonName, 
        //     user.Gender, 
        //     "token", 
        //     true);
        
        return mapper.Map<AuthenticationResponse>(user) with {Success = true, Token = "update Token"};
    }

    public async Task<AuthenticationResponse> Register(RegisterRequest request)
    {
        // ApplicationUser? user = new ApplicationUser()
        // {
        //     PersonName = request.PersonName,
        //     Email = request.Email,
        //     Password = request.Password,
        //     Gender = request.Gender.ToString()
        // };
        ApplicationUser? user = mapper.Map<ApplicationUser>(request);
        
        ApplicationUser? registeredUser = await usersRepository.AddUser(user);
        
        if (registeredUser == null)
        {
            return null;
        }
        
        // Return successful response
        // return new AuthenticationResponse(
        //     registeredUser.UserId, 
        //     registeredUser.Email, 
        //     registeredUser.PersonName, 
        //     registeredUser.Gender, 
        //     "token", 
        //     true);
        
        return mapper.Map<AuthenticationResponse>(registeredUser) with {Success = true, Token = "new Token"};
    }
}
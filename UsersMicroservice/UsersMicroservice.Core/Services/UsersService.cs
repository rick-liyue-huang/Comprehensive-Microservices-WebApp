using AutoMapper;
using UsersMicroservice.Core.Dtos;
using UsersMicroservice.Core.RepositoryContracts;
using UsersMicroservice.Core.ServiceContracts;
using UsersMicroservice.Domain.Dtos;
using UsersMicroservice.Domain.Entities;

namespace UsersMicroservice.Core.Services;

public class UsersService(IUsersRepository usersRepository, IMapper mapper) : IUsersService
{
    public async Task<AuthenticationResponse?> Login(LoginRequest request)
    {
        ApplicationUser user = await usersRepository.GetUserByEmailAndPassword(request.Email, request.Password);

        if (user == null) return null;

        // return new AuthenticationResponse(
            // user.UserId, 
            // user.Email, 
            // user.PersonName, 
            // user.Gender, 
            // "token", 
            // true);
        
        return mapper.Map<AuthenticationResponse>(user) with { Token = "token", Success = true };
    }

    public async Task<AuthenticationResponse?> Register(RegisterRequest request)
    {
        // ApplicationUser user = new ApplicationUser()
        // {
        //     PersonName = request.PersonName,
        //     Email = request.Email,
        //     Password = request.Password,
        //     Gender = request.Gender.ToString()
        // };
        ApplicationUser user = mapper.Map<ApplicationUser>(request);

        ApplicationUser? registeredUser = await usersRepository.AddUser(user);

        if (registeredUser == null) return null;
        
        // return new AuthenticationResponse(
        //     registedUser.UserId, 
        //     registedUser.Email, 
        //     registedUser.PersonName, 
        //     registedUser.Gender, 
        //     "token", 
        //     true);
        
        return mapper.Map<AuthenticationResponse>(registeredUser) with { Token = "token", Success = true };
    }
}
using usersService.Core.Dtos;

namespace usersService.Core.ServiceContracts;

public interface IUsersService
{
    Task<AuthenticationResponse> Login(LoginRequest request);
    Task<AuthenticationResponse> Register(RegisterRequest request);
}
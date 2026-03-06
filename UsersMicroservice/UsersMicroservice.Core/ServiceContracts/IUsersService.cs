using UsersMicroservice.Core.Dtos;
using UsersMicroservice.Domain.Dtos;

namespace UsersMicroservice.Core.ServiceContracts;

public interface IUsersService
{
    Task<AuthenticationResponse?> Login(LoginRequest request);
    Task<AuthenticationResponse?> Register(RegisterRequest request);
}
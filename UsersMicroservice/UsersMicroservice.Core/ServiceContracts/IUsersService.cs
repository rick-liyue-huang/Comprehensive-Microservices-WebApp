using UsersMicroservice.Core.Dtos;
namespace UsersMicroservice.Core.ServiceContracts;

public interface IUsersService
{
  Task<AuthenticationResponse?> Login(LoginRequest loginRequest);
  Task<AuthenticationResponse?> Register(RegisterRequest registerRequest);
}

using UsersMicroservice.Core.Dtos;

namespace UsersMicroservice.Core.ServiceContracts;

public interface IUsersService
{
    /// <summary>
    /// method to handle user login to return the authentication response
    /// </summary>
    /// <param name="loginRequest"></param>
    /// <returns></returns>
    Task<AuthenticationResponse?> Login(LoginRequest loginRequest);

    /// <summary>
    /// method to handle user register use case and returns an object
    /// </summary>
    /// <param name="registerRequest"></param>
    /// <returns></returns>
    Task<AuthenticationResponse?> Register(RegisterRequest registerRequest);
}

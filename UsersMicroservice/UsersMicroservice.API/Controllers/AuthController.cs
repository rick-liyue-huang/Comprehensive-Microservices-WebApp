using Microsoft.AspNetCore.Mvc;
using UsersMicroservice.Core.Dtos;
using UsersMicroservice.Core.ServiceContracts;


namespace UsersMicroservice.API.Controllers;

[ApiController]
[Route("api/[controller]")] // api/auth
public class AuthController(IUsersService usersService) : ControllerBase
{

    [HttpPost("register")] // api/auth/register
    public async Task<IActionResult> Register(RegisterRequest registerRequest)
    {
        // call the UsersService to handle registration
        AuthenticationResponse? authenticationResponse = await usersService.Register(registerRequest);

        if (authenticationResponse == null || authenticationResponse.Success == false)
        {
            return BadRequest(authenticationResponse);
        }

        return Ok(authenticationResponse);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        AuthenticationResponse? authenticationResponse = await usersService.Login(loginRequest);

        if (authenticationResponse == null || authenticationResponse.Success == false)
        {
            return Unauthorized(authenticationResponse);
        }

        return Ok(authenticationResponse);
    }
}

using Microsoft.AspNetCore.Mvc;
using usersService.Core.Dtos;
using usersService.Core.ServiceContracts;

namespace usersService.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AuthController(IUsersService usersService) : ControllerBase
{
    // Endpoint to register a new user
    [Route("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        if (request == null)
        {
            return BadRequest("Invalid registration request.");
        }
        
        AuthenticationResponse? response = await usersService.Register(request);

        if (response == null || response.Success == false)
        {
            return BadRequest(response);
        }
        
        return Ok(response);
    }

    [Route("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        if (request == null)
        {
            return BadRequest("Invalid login request");
        }
        
        AuthenticationResponse? response = await usersService.Login(request);
        
        if (response == null || response.Success == false)
        {
            return Unauthorized(response);
        }
        
        return Ok(response);
    }
    
}
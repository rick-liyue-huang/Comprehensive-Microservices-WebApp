using Microsoft.AspNetCore.Mvc;
using UsersMicroservice.Core.Dtos;
using UsersMicroservice.Core.ServiceContracts;
using UsersMicroservice.Domain.Dtos;

namespace UsersMicroservice.API.Controllers;

[Route("api/[controller]")] // api/auth
[ApiController]
public class AuthController(IUsersService usersService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        if (request == null) return BadRequest("Invalid request on register");
        
        AuthenticationResponse? response = await usersService.Register(request);
        
        if (response == null || response.Success == false) return BadRequest(response);
        
        return Ok(response);
        
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        if (request == null) return BadRequest("Invalid request on login");
        
        AuthenticationResponse? response = await usersService.Login(request);
        
        if (response == null || response.Success == false) return Unauthorized(response);
        
        return Ok(response);
    }
}
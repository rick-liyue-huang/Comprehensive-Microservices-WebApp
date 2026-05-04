using Microsoft.AspNetCore.Mvc;
using UsersMicroservice.Core.Dtos;
using UsersMicroservice.Core.ServiceContracts;


namespace UsersMicroservice.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IUsersService usersService) : ControllerBase
{
  [HttpPost("register")] // POST api/auth/register
  public async Task<IActionResult> Register(RegisterRequest registerRequest)
  {
    if (registerRequest == null)
      return BadRequest("Invalid request");

    AuthenticationResponse? response = await usersService.Register(registerRequest);
    if (response == null || response.Success is false)
    {
      return BadRequest(response);
    }
    return Ok(response);
  }

  [HttpPost("login")] // POST api/auth/login
  public async Task<IActionResult> Login(LoginRequest loginRequest)
  {
    if (loginRequest == null)
    {
      return BadRequest("Invalid request");
    }
    AuthenticationResponse? response = await usersService.Login(loginRequest);

    if (response == null || response.Success is false)
    {
      return Unauthorized(response);
    }
    return Ok(response);
  }
}

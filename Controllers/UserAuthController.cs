using Kruggers_Backend.Data;
using Kruggers_Backend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Kruggers_Backend.Controllers;

[ApiController]
[Route("api/v1/userauth/")]
public class UserAuthController : ControllerBase
{
    private readonly IUserAuthRepository _userAuthRepository;

    public UserAuthController(IUserAuthRepository userAuthRepository)
    {
        _userAuthRepository = userAuthRepository;
    }

    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody]UserRegistrationDTO dto)
    {
        var result = await _userAuthRepository.CreateUserAsync(dto);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserLoginResponseDTO>> Login([FromBody] UserLoginDTO dto)
    {
        if (dto == null)
        {
            return BadRequest("User Is Null");
        }

        var userResponse =  await _userAuthRepository.LoginUser(dto);
        if (userResponse == null)
        {
            return Unauthorized("User not found / Validated");
        }
        return Ok(userResponse);
       
    }
}
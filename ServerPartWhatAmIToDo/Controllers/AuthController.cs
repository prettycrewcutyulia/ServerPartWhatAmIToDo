using Microsoft.AspNetCore.Mvc;
using ServerPartWhatAmIToDo.Models;
using ServerPartWhatAmIToDo.Models.Goals;
using ServerPartWhatAmIToDo.Services;

namespace ServerPartWhatAmIToDo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{

    private IUserService _userService;
    public AuthController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken token)
    {
        var result = await _userService.Login(email: request.Email, password: request.Password, token);
        // Проверка логина и пароля пользователя
        if (result.Item1)
        {
            return Ok(result.Item2);
        }
        return BadRequest("Login failed");
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken token)
    {
        try
        {
           var userId = await _userService.AddUserAsync(request, token);
            
           var tokenString = await TokenService.GenerateToken(request.Email);
            
           var result = new LoginResponse(userId, request.Nickname, request.Email, tokenString);
            return Ok(result);
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            return BadRequest("Registration failed");
        }
    }
}
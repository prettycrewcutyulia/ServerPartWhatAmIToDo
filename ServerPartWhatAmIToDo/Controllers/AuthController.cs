using Microsoft.AspNetCore.Authorization;
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
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var result = _userService.Login(email: request.Email, password: request.Password).Result;
        // Проверка логина и пароля пользователя
        if (result.Item1)
        {
            return Ok(result.Item2);
        }
        return Unauthorized();
    }
    
    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterRequest request)
    {
        try
        {
           var userId = _userService.AddUserAsync(request).Result;
            
           var tokenString = TokenService.GenerateToken(request.Email).Result;
            
           var result = new LoginResponse(userId, request.Nickname, request.Email, tokenString);
            return Ok(result);
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            return BadRequest("Registration failed");
        }
    }
    
    // [HttpPost("resetpassword")]
    // public IActionResult ResetPassword([FromBody] ResetPasswordRequest request)
    // {
    //     
    //     var resetPasswordUrl = Url.Action("ResetPasswordView", "Auth", new { }, Request.Scheme);
    //
    //     return Ok(new { Message = "Password reset link sent", ResetLink = resetPasswordUrl });
    // }
    //
    // [HttpGet("resetpassword")]
    // public IActionResult ResetPassword()
    // {
    //     // Возвращает представление для сброса пароля,
    //     // если вы планируете обслуживать HTML страницу.
    //     return  Microsoft.AspNetCore.Mvc.Controller.View("ResetPasswordView");
    // }
    
}
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
        var result = await _userService.Login(email: request.Email, password: request.Password);
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
           var userId = await _userService.AddUserAsync(request);
            
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
    
    // [HttpPost("send-password-reset")]
    // public IActionResult SendPasswordResetEmail([FromBody] string email)
    // {
    //     if (string.IsNullOrWhiteSpace(email))
    //     {
    //         return BadRequest("Email must be provided.");
    //     }
    //
    //     // Вызов метода отправки письма
    //     SendPasswordResetEmailService(email);
    //
    //     return Ok("Password reset email sent.");
    // }
    //
    // public void SendPasswordResetEmailService(string toEmail)
    // {
    //     string fromEmail = "gudoshnikova11@gmail.com";
    //     string fromPassword = "TOP27u12g2002";
    //     string subject = "Password Reset Request";
    //     string body = $"Please reset your password using the following link: 1111111";
    //
    //     var smtpClient = new SmtpClient("smtp.gmail.com")
    //     {
    //         Port = 587, // или другой порт, в зависимости от вашего SMTP сервера
    //         Credentials = new NetworkCredential(fromEmail, fromPassword),
    //         EnableSsl = true,
    //     };
    //
    //     try
    //     {
    //         smtpClient.Send(fromEmail, toEmail, subject, body);
    //         Console.WriteLine("Password reset email sent successfully.");
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine($"Error sending email: {ex.Message}");
    //     }
    // }
}
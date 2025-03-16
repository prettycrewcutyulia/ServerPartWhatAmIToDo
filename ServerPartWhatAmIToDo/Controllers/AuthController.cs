using Microsoft.AspNetCore.Mvc;
using ServerPartWhatAmIToDo.Models;

namespace ServerPartWhatAmIToDo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        // Проверка логина и пароля пользователя
        if (request.Email == "test@example.com" && request.Password == "password")
        {
            // Генерация токена
            var token = "generated_jwt_token"; // Замените на ваш механизм генерации токена
            return Ok(new { Token = token });
        }
        return Unauthorized();
    }
    
    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterRequest request)
    {
        // Логика регистрации
        bool isRegistered = true; // Допустим пользователю удалось зарегистрироваться
        if (isRegistered)
        {
            // Генерация токена
            var token = "generated_jwt_token";
            return Ok(new { Message = "Registration successful", Token = token });
        }
        return BadRequest("Registration failed");
    }
    
    [HttpPost("resetpassword")]
    public IActionResult ResetPassword([FromBody] ResetPasswordRequest request)
    {
        // Логика сброса пароля
        var resetLink = "https://example.com/reset-password?token=reset_token";
        return Ok(new { Message = "Password reset link sent", ResetLink = resetLink });
    }
    
    [HttpPost("logout")]
    public IActionResult Logout([FromBody] DeleteAccountRequest request)
    {
        // Логика разлогина, например, добавление токена в черный список
        // или удаление из текущей сессии.

        return Ok(new { Message = "Logout successful" });
    }
    
    [HttpDelete("deleteaccount")]
    public IActionResult DeleteAccount([FromBody] DeleteAccountRequest request)
    {
        // Логика для удаления аккаунта
        var userDeleted = DeleteUserAccount(request.UserId);

        if (userDeleted)
        {
            return Ok(new { Message = "Account successfully deleted" });
        }

        return BadRequest("Failed to delete account");
    }

    private bool DeleteUserAccount(string userId)
    {
        // Реализуйте логику для удаления пользователя из базы данных
        // Пример: возвращаем true, если успешно удалено
        return true;
    }
}
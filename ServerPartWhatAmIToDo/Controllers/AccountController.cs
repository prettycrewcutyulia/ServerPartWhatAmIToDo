using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServerPartWhatAmIToDo.Models;
using ServerPartWhatAmIToDo.Services;

namespace ServerPartWhatAmIToDo.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IUserService _userService;

    public AccountController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpDelete("update")]
    public IActionResult UpdateAccount([FromBody] UpdateAccountRequest request)
    {
        try
        {
            // Логика для удаления аккаунта
            _userService.UpdateUserAsync(request).Wait();
            return Ok(new { Message = "Account successfully deleted" });
        }
        catch
        {
            return BadRequest("Account update failed");
        }
    }
    
    [HttpDelete("delete")]
    public IActionResult DeleteAccount([FromBody] DeleteAccountRequest request)
    {
        try
        {
            // Логика для удаления аккаунта
            var userDeleted = _userService.DeleteUserAsync(request.UserId);
            return Ok(new { Message = "Account successfully deleted" });
        }
        catch
        {
            return BadRequest("Account deletion failed");
        }
    }
    
    [HttpPut("update/tg")]
    public IActionResult UpdateTg([FromBody] UpdateTgRequest request)
    {
        try
        {
            // Логика для удаления аккаунта
            _userService.UpdateUserAsync(request).Wait();
            return Ok(new { Message = "Tg added successfully" });
        }
        catch
        {
            return BadRequest("Tg update failed");
        }
    }
    
    [HttpPut("correct")]
    public IActionResult VerifyUser([FromBody] string email)
    {
        try
        {
            // Логика для удаления аккаунта
            var user = _userService.GetUserByEmailAsync(email).Result;
            if (user == null)
            {
                return BadRequest("Invalid email");
            }
            else
            {
                return Ok();
            }
        }
        catch
        {
            return BadRequest("Invalid email");
        }
    }
}
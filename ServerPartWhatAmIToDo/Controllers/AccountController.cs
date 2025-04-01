using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServerPartWhatAmIToDo.Models;
using ServerPartWhatAmIToDo.Services;

namespace ServerPartWhatAmIToDo.Controllers;

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
    
    [Authorize]
    [HttpPut("correct")]
    public IActionResult VerifyUser([FromBody] UpdateTgRequest request)
    {
        try
        {
            var user = _userService.GetUserByEmailAsync(request.Email).Result;
            if (user != null)
            {

                _userService.UpdateUserAsync(request).Wait();
                return Ok();
            }
            
            return BadRequest("Invalid email");
            
        }
        catch
        {
            return BadRequest("Invalid email");
        }
    }
    
    [Authorize]
    [HttpGet("tgExist/{id}")]
    public IActionResult GetTgExist([FromQuery] int userId) {
        var res = _userService.ExistTgUserAsync(userId).Result;

        if (res)
        {
            return Ok();
        }
        else
        {
            return NotFound();
        }
    }
}
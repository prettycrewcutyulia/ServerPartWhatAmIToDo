using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MailKit.Net.Smtp;
using ServerPartWhatAmIToDo.Models;
using ServerPartWhatAmIToDo.Services;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace ServerPartWhatAmIToDo.Controllers;

// [Authorize]
[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IUserService _userService;

    public AccountController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpPut("update")]
    public async Task<IActionResult> UpdateAccount([FromBody] UpdateAccountRequest request, CancellationToken token)
    {
        try
        {
            // Логика для удаления аккаунта
            await _userService.UpdateUserAsync(request);
            return Ok(new { Message = "Account successfully updated" });
        }
        catch
        {
            return BadRequest("Account update failed");
        }
    }
    
    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteAccount(
        [FromBody] DeleteAccountRequest request, 
        CancellationToken token
        )
    {
        try
        {
            // Логика для удаления аккаунта
            await _userService.DeleteUserAsync(request.UserId);
            return Ok(new { Message = "Account successfully deleted" });
        }
        catch
        {
            return BadRequest("Account deletion failed");
        }
    }
    
    [HttpPut("update/tg")]
    public async Task<IActionResult> UpdateTg([FromBody] UpdateTgRequest request, CancellationToken token)
    {
        try
        {
            // Логика для удаления аккаунта
            await _userService.UpdateUserAsync(request);
            return Ok(new { Message = "Tg added successfully" });
        }
        catch
        {
            return BadRequest("Tg update failed");
        }
    }
    
    [Authorize]
    [HttpPut("correct")]
    public async Task<IActionResult> VerifyUser([FromBody] UpdateTgRequest request, CancellationToken token)
    {
        try
        {
            var user = await _userService.GetUserByEmailAsync(request.Email);
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
    public async Task<IActionResult> GetTgExist([FromQuery] int userId, CancellationToken token) {
        var res = await _userService.ExistTgUserAsync(userId);

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
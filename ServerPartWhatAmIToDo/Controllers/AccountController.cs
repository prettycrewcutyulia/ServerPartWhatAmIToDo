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
    
    [HttpPut("update")]
    public async Task<IActionResult> UpdateAccount([FromBody] UpdateAccountRequest request, CancellationToken token)
    {
        try
        {
            // Логика для удаления аккаунта
            await _userService.UpdateUserAsync(request, token);
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
            await _userService.DeleteUserAsync(request.UserId, token);
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
            // Логика для обновления аккаунта
            await _userService.UpdateUserAsync(request, token);
            return Ok(new { Message = "Tg added successfully" });
        }
        catch
        {
            return BadRequest("Tg update failed");
        }
    }
    
    [HttpPut("correct")]
    public async Task<IActionResult> VerifyUser([FromBody] UpdateTgRequest request, CancellationToken token)
    {
        try
        {
            var user = await _userService.GetUserByEmailAsync(request.Email, token);
            if (user != null)
            {

                await _userService.UpdateUserAsync(request, token);
                return Ok();
            }
            
            return BadRequest("Invalid email");
            
        }
        catch
        {
            return BadRequest("Invalid email");
        }
    }

    [HttpGet("tgExist/{id}")]
    public async Task<IActionResult> GetTgExist(int id, CancellationToken token) {
        var res = await _userService.ExistTgUserAsync(id, token);

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
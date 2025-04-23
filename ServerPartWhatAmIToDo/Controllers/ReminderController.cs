using Microsoft.AspNetCore.Authorization;
using ServerPartWhatAmIToDo.Models;
using ServerPartWhatAmIToDo.Services;

namespace ServerPartWhatAmIToDo.Controllers;


using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class RemindersController : ControllerBase
{
    private readonly IReminderService _reminderService;

    public RemindersController(IReminderService reminderService)
    {
        _reminderService = reminderService;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetRemindersByUserId(int userId, CancellationToken token)
    {
        var reminders =  await _reminderService.GetRemindersByUserIdAsync(userId);
        return Ok(reminders);
    }

    [HttpPost]
    public async Task<IActionResult> AddReminder([FromBody] ReminderRequest reminder, CancellationToken token)
    {
        if (reminder == null)
        {
            return BadRequest("Reminder cannot be null.");
        }

        int reminderId = await _reminderService.AddReminderAsync(reminder);
        return Ok(reminderId);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReminder(int id, CancellationToken token)
    { 
        await _reminderService.DeleteReminderAsync(id);
        return Ok();
    }
}

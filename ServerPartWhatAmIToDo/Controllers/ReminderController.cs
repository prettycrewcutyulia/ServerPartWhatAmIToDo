using ServerPartWhatAmIToDo.Models;
using ServerPartWhatAmIToDo.Services;

namespace ServerPartWhatAmIToDo.Controllers;


using Microsoft.AspNetCore.Mvc;

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
    public IActionResult GetRemindersByUserId(int userId)
    {
        var reminders =  _reminderService.GetRemindersByUserIdAsync(userId).Result;
        return Ok(reminders);
    }

    [HttpPost]
    public IActionResult AddReminder([FromBody] ReminderRequest reminder)
    {
        if (reminder == null)
        {
            return BadRequest("Reminder cannot be null.");
        }

        int reminderId = _reminderService.AddReminderAsync(reminder).Result;
        return Ok(reminderId);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteReminder(int id)
    { 
        _reminderService.DeleteReminderAsync(id).Wait();
        return Ok();
    }
}

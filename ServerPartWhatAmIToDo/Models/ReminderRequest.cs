namespace ServerPartWhatAmIToDo.Models;

public class ReminderRequest
{
    public int UserId { get; set; }
    public int CountDays { get; set; }
}
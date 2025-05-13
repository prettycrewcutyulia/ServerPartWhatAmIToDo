namespace ServerPartWhatAmIToDo.Models;

public class ReminderRequest
{
    public int UserId { get; set; }
    public int DaysCount { get; set; }
}
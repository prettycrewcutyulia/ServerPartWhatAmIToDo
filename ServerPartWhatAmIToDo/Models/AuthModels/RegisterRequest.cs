namespace ServerPartWhatAmIToDo.Models;

public class RegisterRequest
{
    public string Nickname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
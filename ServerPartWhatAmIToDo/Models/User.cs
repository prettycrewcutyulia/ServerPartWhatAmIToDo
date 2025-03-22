namespace ServerPartWhatAmIToDo.Models;

public class User
{
    public User(string nickname, string email, string password)
    {
        Name = nickname;
        Email = email;
        Password = password;
    }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string? tgId { get; set; }
}
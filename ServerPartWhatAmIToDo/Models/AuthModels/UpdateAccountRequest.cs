namespace ServerPartWhatAmIToDo.Models;

public class UpdateAccountRequest
{
    public int UserId { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
}

public class UpdateTgRequest
{
    public long TgId { get; set; }
    public string Email { get; set; }
}
namespace ServerPartWhatAmIToDo.Models.Goals;


public class GoalResponse
{
    public string Id { get; set; }
    public string Title { get; set; }
    public List<string> CategoriesId { get; set; }
    public List<StepRequest> Steps { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? Deadline { get; set; }
}

public class StepResponse
{
    public string Id { get; set; }
    public string Title { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? Deadline { get; set; }
}

public class UpdateFilterResponse
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Color { get; set; }
}

public class LoginResponse
{
    
    public long Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }

    public LoginResponse(long Id, string Name, string Email, string Token)
    {
        this.Id = Id;
        this.Name = Name;
        this.Email = Email;
        this.Token = Token;
    }
}
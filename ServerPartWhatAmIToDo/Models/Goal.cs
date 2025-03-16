namespace ServerPartWhatAmIToDo.Models;

public class Goal
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}

public class UpdateGoalRequest
{
    public string Title { get; set; }
    public string Description { get; set; }
}

public class AiGoalRequest
{
    public string Context { get; set; }
}
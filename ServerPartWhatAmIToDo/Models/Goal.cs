namespace ServerPartWhatAmIToDo.Models;

public class GoalRequest
{
    public string UserId { get; set; }
    public string Title { get; set; }
}
public class Goal
{
    public string Id { get; set; }
    
    public string UserId { get; set; }
    public string Title { get; set; }

    public Goal(GoalRequest goal)
    {
        Id = Guid.NewGuid().ToString();
        UserId = goal.UserId;
        Title = goal.Title;
    }
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
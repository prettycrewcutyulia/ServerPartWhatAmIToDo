namespace ServerPartWhatAmIToDo.Models.Goals;

public class GoalRequest
{
    public int UserId { get; set; }
    public string Title { get; set; }
    public List<int> CategoriesId { get; set; }
    public List<StepRequest> Steps { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? Deadline { get; set; }
}

public class StepRequest
{
    public int? StepId { get; set; }
    public string Title { get; set; }
    public bool IsCompleted { get; set; }
    
    public DateTime? Deadline { get; set; }
}

public class AiGoalRequest
{
    public string Context { get; set; }
}

public class UpdateFilterRequest
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Color { get; set; }
}

public class FilterRequest
{
    public string Title { get; set; }
    public string Color { get; set; }
}
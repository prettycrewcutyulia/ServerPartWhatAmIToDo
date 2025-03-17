namespace ServerPartWhatAmIToDo.Models.Goals;

public class GoalRequest
{
    // Убрали поле Id, так как оно не нужно для обработки запросов
    public string UserId { get; set; }
    public string Title { get; set; }
    public List<CategoryRequest> Categories { get; set; }
    public List<StepRequest> Steps { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? Deadline { get; set; }
}

public class StepRequest
{
    // Убрали поле Id, так как оно не нужно для обработки запросов
    public string Title { get; set; }
    public bool IsCompleted { get; set; }
}

public class CategoryRequest
{
    public string Id { get; set; }
}

public class AiGoalRequest
{
    public string Context { get; set; }
}

public class UpdateFilterRequest
{
    public string Title { get; set; }
    public string Color { get; set; }
}
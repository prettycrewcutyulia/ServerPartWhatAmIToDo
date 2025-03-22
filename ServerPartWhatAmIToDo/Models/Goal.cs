using Newtonsoft.Json;
using ServerPartWhatAmIToDo.Models.DataBase;
using ServerPartWhatAmIToDo.Models.Goals;

namespace ServerPartWhatAmIToDo.Models;

public class Goal
{
    public int? Id { get; set; }
    
    public int UserId { get; set; }
    public string Title { get; set; }
    
    public List<int> CategoryId { get; set; }
    
    public List<Step> Steps { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? Deadline { get; set; }
    

    public Goal(GoalRequest goal)
    {
        Id = null;
        UserId = goal.UserId;
        Title = goal.Title;
        CategoryId = goal.CategoriesId;
        Steps = goal.Steps.Select(c=> new Step(c)).ToList();
        StartDate = goal.StartDate;
        Deadline = goal.Deadline;
    }

    public Goal(GoalEntity goal, IEnumerable<StepEntity> steps)
    {
        Id = goal.GoalId;
        Title = goal.Title;
        UserId = goal.UserId;
        CategoryId = goal.IdFilters.ToList();
        StartDate = goal.StartDate;
        Deadline = goal.Deadline;
        Steps = steps.Select(c=> new Step(c)).ToList();
    }
}

public class Step
{
    public Step(StepRequest stepRequest)
    {
        Id = null;
        Title = stepRequest.Title;
        IsCompleted = stepRequest.IsCompleted;
        Deadline = stepRequest.Deadline;
    }
    
    public Step(StepEntity step)
    {
        Id = step.StepId;
        Title = step.Title;
        IsCompleted = step.IsCompleted ?? false;
        Deadline = step.Deadline;
    }

    public int? Id { get; set; }
    public string Title { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? Deadline { get; set; }
}
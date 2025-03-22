using ServerPartWhatAmIToDo.Models.DataBase;

namespace ServerPartWhatAmIToDo.Repositories.Protocols
{
    public interface IGoalRepository
    {
        Task<IEnumerable<GoalEntity>> GetAllGoalsAsync();
        Task<GoalEntity> GetGoalByIdAsync(int goalId);
        Task<IEnumerable<GoalEntity>> GetGoalsByUserIdAsync(int userId);
        Task<IEnumerable<StepEntity>> GetStepsByGoalIdAsync(int goalId);
        Task<int> AddGoalAsync(GoalEntity goal);
        Task UpdateGoalAsync(GoalEntity goal);
        Task DeleteGoalAsync(int goalId);
    }
}
using ServerPartWhatAmIToDo.Models.DataBase;

namespace ServerPartWhatAmIToDo.Repositories.Protocols
{
    public interface IGoalRepository
    {
        Task<IEnumerable<GoalEntity>> GetAllGoalsAsync(CancellationToken cancellationToken);
        Task<GoalEntity> GetGoalByIdAsync(int goalId, CancellationToken cancellationToken);
        Task<IEnumerable<GoalEntity>> GetGoalsByUserIdAsync(int userId, CancellationToken cancellationToken);
        Task<IEnumerable<StepEntity>> GetStepsByGoalIdAsync(int goalId, CancellationToken cancellationToken);
        Task<int> AddGoalAsync(GoalEntity goal, CancellationToken cancellationToken);
        Task UpdateGoalAsync(GoalEntity goal, CancellationToken cancellationToken);
        Task DeleteGoalAsync(int goalId, CancellationToken cancellationToken);
    }
}
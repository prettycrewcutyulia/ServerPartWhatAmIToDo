using ServerPartWhatAmIToDo.Models.DataBase;

namespace ServerPartWhatAmIToDo.Repositories.Protocols
{
    public interface IDeadlineRepository
    {
        Task<IEnumerable<DeadlineEntity>> GetAllDeadlinesAsync(CancellationToken cancellationToken);
        Task<DeadlineEntity> GetDeadlineByIdAsync(int deadlineId, CancellationToken cancellationToken);
        
        Task<IEnumerable<DeadlineEntity>> GetDeadlinesByUserIdAsync(int userId, CancellationToken cancellationToken);
        Task<IEnumerable<DeadlineEntity>> GetDeadlinesByGoalIdAsync(int goalId, CancellationToken cancellationToken);
        Task<IEnumerable<DeadlineEntity>> GetDeadlinesByStepIdAsync(int stepId, CancellationToken cancellationToken);
        Task<int> AddDeadlineAsync(DeadlineEntity deadline, CancellationToken cancellationToken);
        Task UpdateDeadlineAsync(DeadlineEntity deadline, CancellationToken cancellationToken);
        Task DeleteDeadlineAsync(int deadlineId, CancellationToken cancellationToken);
        Task<IEnumerable<DeadlineEntity>> GetDeadlinesForUserAsync(int userId, int maxDaysCount, CancellationToken cancellationToken);
    }
}
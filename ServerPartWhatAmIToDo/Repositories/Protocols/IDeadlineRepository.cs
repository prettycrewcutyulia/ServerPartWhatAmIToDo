using ServerPartWhatAmIToDo.Models.DataBase;

namespace ServerPartWhatAmIToDo.Repositories.Protocols
{
    public interface IDeadlineRepository
    {
        Task<IEnumerable<DeadlineEntity>> GetAllDeadlinesAsync();
        Task<DeadlineEntity> GetDeadlineByIdAsync(int deadlineId);
        
        Task<IEnumerable<DeadlineEntity>> GetDeadlinesByUserIdAsync(int userId);
        Task<IEnumerable<DeadlineEntity>> GetDeadlinesByGoalIdAsync(int goalId);
        Task<IEnumerable<DeadlineEntity>> GetDeadlinesByStepIdAsync(int stepId);
        Task<int> AddDeadlineAsync(DeadlineEntity deadline);
        Task UpdateDeadlineAsync(DeadlineEntity deadline);
        Task DeleteDeadlineAsync(int deadlineId);
    }
}
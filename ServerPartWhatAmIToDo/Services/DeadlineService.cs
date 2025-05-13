using ServerPartWhatAmIToDo.Models.DataBase;
using ServerPartWhatAmIToDo.Repositories.Protocols;

namespace ServerPartWhatAmIToDo.Services
{
    public interface IDeadlineService
    {
        Task<IEnumerable<DeadlineEntity>> GetAllDeadlinesAsync(CancellationToken cancellationToken);
        Task<DeadlineEntity> GetDeadlineByIdAsync(int deadlineId, CancellationToken cancellationToken);
        Task<IEnumerable<DeadlineEntity>> GetDeadlinesByGoalIdAsync(int goalId, CancellationToken cancellationToken);
        Task<IEnumerable<DeadlineEntity>> GetDeadlinesByStepIdAsync(int stepId, CancellationToken cancellationToken);
        Task<int> AddDeadlineAsync(DeadlineEntity deadline, CancellationToken cancellationToken);
        Task UpdateDeadlineAsync(DeadlineEntity deadline, CancellationToken cancellationToken);
        Task DeleteDeadlineAsync(int deadlineId, CancellationToken cancellationToken);
        Task<IEnumerable<DeadlineEntity>> GetDeadlinesForUserAsync(int userId, int maxDaysCount, CancellationToken cancellationToken);
    }
    
    public class DeadlineService : IDeadlineService
    {
        private readonly IDeadlineRepository _deadlineRepository;

        public DeadlineService(IDeadlineRepository deadlineRepository)
        {
            _deadlineRepository = deadlineRepository;
        }

        public async Task<IEnumerable<DeadlineEntity>> GetAllDeadlinesAsync(CancellationToken cancellationToken)
        {
            return await _deadlineRepository.GetAllDeadlinesAsync(cancellationToken);
        }

        public async Task<DeadlineEntity> GetDeadlineByIdAsync(int deadlineId, CancellationToken cancellationToken)
        {
            return await _deadlineRepository.GetDeadlineByIdAsync(deadlineId, cancellationToken);
        }

        public async Task<IEnumerable<DeadlineEntity>> GetDeadlinesByGoalIdAsync(int goalId, CancellationToken cancellationToken)
        {
            return await _deadlineRepository.GetDeadlinesByGoalIdAsync(goalId, cancellationToken);
        }

        public async Task<IEnumerable<DeadlineEntity>> GetDeadlinesByStepIdAsync(int stepId, CancellationToken cancellationToken)
        {
            return await _deadlineRepository.GetDeadlinesByStepIdAsync(stepId, cancellationToken);
        }

        public async Task<int> AddDeadlineAsync(DeadlineEntity deadline, CancellationToken cancellationToken)
        {
            return await _deadlineRepository.AddDeadlineAsync(deadline, cancellationToken);
        }

        public async Task UpdateDeadlineAsync(DeadlineEntity deadline, CancellationToken cancellationToken)
        {
            await _deadlineRepository.UpdateDeadlineAsync(deadline, cancellationToken);
        }

        public async Task DeleteDeadlineAsync(int deadlineId, CancellationToken cancellationToken)
        {
            await _deadlineRepository.DeleteDeadlineAsync(deadlineId, cancellationToken);
        }

        public async Task<IEnumerable<DeadlineEntity>> GetDeadlinesForUserAsync(int userId, int maxDaysCount, CancellationToken cancellationToken)
        {
           return await _deadlineRepository.GetDeadlinesForUserAsync(userId, maxDaysCount, cancellationToken);
        }
    }
}
using ServerPartWhatAmIToDo.Models.DataBase;
using ServerPartWhatAmIToDo.Repositories.Protocols;

namespace ServerPartWhatAmIToDo.Services
{
    public interface IDeadlineService
    {
        Task<IEnumerable<DeadlineEntity>> GetAllDeadlinesAsync();
        Task<DeadlineEntity> GetDeadlineByIdAsync(int deadlineId);
        Task<IEnumerable<DeadlineEntity>> GetDeadlinesByGoalIdAsync(int goalId);
        Task<IEnumerable<DeadlineEntity>> GetDeadlinesByStepIdAsync(int stepId);
        Task<int> AddDeadlineAsync(DeadlineEntity deadline);
        Task UpdateDeadlineAsync(DeadlineEntity deadline);
        Task DeleteDeadlineAsync(int deadlineId);
    }
    
    public class DeadlineService : IDeadlineService
    {
        private readonly IDeadlineRepository _deadlineRepository;

        public DeadlineService(IDeadlineRepository deadlineRepository)
        {
            _deadlineRepository = deadlineRepository;
        }

        public async Task<IEnumerable<DeadlineEntity>> GetAllDeadlinesAsync()
        {
            return await _deadlineRepository.GetAllDeadlinesAsync();
        }

        public async Task<DeadlineEntity> GetDeadlineByIdAsync(int deadlineId)
        {
            return await _deadlineRepository.GetDeadlineByIdAsync(deadlineId);
        }

        public async Task<IEnumerable<DeadlineEntity>> GetDeadlinesByGoalIdAsync(int goalId)
        {
            return await _deadlineRepository.GetDeadlinesByGoalIdAsync(goalId);
        }

        public async Task<IEnumerable<DeadlineEntity>> GetDeadlinesByStepIdAsync(int stepId)
        {
            return await _deadlineRepository.GetDeadlinesByStepIdAsync(stepId);
        }

        public async Task<int> AddDeadlineAsync(DeadlineEntity deadline)
        {
            // Вы можете добавить валидацию или бизнес-логику перед добавлением дедлайна
            return await _deadlineRepository.AddDeadlineAsync(deadline);
        }

        public async Task UpdateDeadlineAsync(DeadlineEntity deadline)
        {
            // Проведите необходимые проверки или валидацию перед обновлением дедлайна
            await _deadlineRepository.UpdateDeadlineAsync(deadline);
        }

        public async Task DeleteDeadlineAsync(int deadlineId)
        {
            // Перед удалением проверьте, существует ли дедлайн или добавьте другие условия
            await _deadlineRepository.DeleteDeadlineAsync(deadlineId);
        }
    }
}
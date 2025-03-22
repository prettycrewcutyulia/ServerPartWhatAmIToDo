
using ServerPartWhatAmIToDo.Models;
using ServerPartWhatAmIToDo.Models.DataBase;
using ServerPartWhatAmIToDo.Repositories.Protocols;

namespace ServerPartWhatAmIToDo.Services
{
    public interface IStepService
    {
        Task<IEnumerable<StepEntity>> GetAllStepsAsync();
        Task<StepEntity?> GetStepByIdAsync(int stepId);
        Task<int> AddStepAsync(Step step);
        Task UpdateStepAsync(StepEntity step);
        Task DeleteStepAsync(int stepId);
    }
    
    public class StepService : IStepService
    {
        private readonly IStepRepository _stepRepository;

        public StepService(IStepRepository stepRepository)
        {
            _stepRepository = stepRepository;
        }

        public async Task<IEnumerable<StepEntity>> GetAllStepsAsync()
        {
            return await _stepRepository.GetAllStepsAsync();
        }

        public async Task<StepEntity?> GetStepByIdAsync(int stepId)
        {
            return await _stepRepository.GetStepByIdAsync(stepId);
        }

        public async Task<int> AddStepAsync(Step step)
        {
            var entity = new StepEntity();
            entity.Title = step.Title;
            entity.IsCompleted = step.IsCompleted;
            entity.Deadline = step.Deadline;
            return await _stepRepository.AddStepAsync(entity);
        }

        public async Task UpdateStepAsync(StepEntity step)
        {
            // Здесь можно добавить проверку существования шага и бизнес-логику перед обновлением
            await _stepRepository.UpdateStepAsync(step);
        }

        public async Task DeleteStepAsync(int stepId)
        {
            // Здесь можно добавить проверку существования шага перед удалением или другие условия
            await _stepRepository.DeleteStepAsync(stepId);
        }
    }
}

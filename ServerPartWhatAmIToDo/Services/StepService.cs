
using ServerPartWhatAmIToDo.Models;
using ServerPartWhatAmIToDo.Models.DataBase;
using ServerPartWhatAmIToDo.Repositories.Protocols;

namespace ServerPartWhatAmIToDo.Services
{
    public interface IStepService
    {
        Task<IEnumerable<StepEntity>> GetAllStepsAsync(CancellationToken cancellationToken);
        Task<StepEntity?> GetStepByIdAsync(int stepId, CancellationToken cancellationToken);
        Task<int> AddStepAsync(Step step, CancellationToken cancellationToken);
        Task UpdateStepAsync(StepEntity step, CancellationToken cancellationToken);
        Task DeleteStepAsync(int stepId, CancellationToken cancellationToken);
    }
    
    public class StepService : IStepService
    {
        private readonly IStepRepository _stepRepository;

        public StepService(IStepRepository stepRepository)
        {
            _stepRepository = stepRepository;
        }

        public async Task<IEnumerable<StepEntity>> GetAllStepsAsync(CancellationToken cancellationToken)
        {
            return await _stepRepository.GetAllStepsAsync(cancellationToken);
        }

        public async Task<StepEntity?> GetStepByIdAsync(int stepId, CancellationToken cancellationToken)
        {
            return await _stepRepository.GetStepByIdAsync(stepId, cancellationToken);
        }

        public async Task<int> AddStepAsync(Step step, CancellationToken cancellationToken)
        {
            var entity = new StepEntity();
            entity.Title = step.Title;
            entity.IsCompleted = step.IsCompleted;
            entity.Deadline = step.Deadline;
            return await _stepRepository.AddStepAsync(entity, cancellationToken);
        }

        public async Task UpdateStepAsync(StepEntity step, CancellationToken cancellationToken)
        {
            await _stepRepository.UpdateStepAsync(step, cancellationToken);
        }

        public async Task DeleteStepAsync(int stepId, CancellationToken cancellationToken)
        {
            await _stepRepository.DeleteStepAsync(stepId, cancellationToken);
        }
    }
}

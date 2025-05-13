using ServerPartWhatAmIToDo.Models.DataBase;

namespace ServerPartWhatAmIToDo.Repositories.Protocols
{
    public interface IStepRepository
    {
        Task<IEnumerable<StepEntity>> GetAllStepsAsync(CancellationToken cancellationToken);
        Task<StepEntity> GetStepByIdAsync(int stepId, CancellationToken cancellationToken);
        Task<int> AddStepAsync(StepEntity step, CancellationToken cancellationToken);
        Task UpdateStepAsync(StepEntity step, CancellationToken cancellationToken);
        Task DeleteStepAsync(int stepId, CancellationToken cancellationToken);
    }
}
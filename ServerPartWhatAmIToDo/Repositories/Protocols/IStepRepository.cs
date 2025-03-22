using ServerPartWhatAmIToDo.Models.DataBase;

namespace ServerPartWhatAmIToDo.Repositories.Protocols
{
    public interface IStepRepository
    {
        Task<IEnumerable<StepEntity>> GetAllStepsAsync();
        Task<StepEntity> GetStepByIdAsync(int stepId);
        Task<int> AddStepAsync(StepEntity step);
        Task UpdateStepAsync(StepEntity step);
        Task DeleteStepAsync(int stepId);
    }
}
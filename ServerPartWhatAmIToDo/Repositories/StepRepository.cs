using Microsoft.EntityFrameworkCore;
using ServerPartWhatAmIToDo.Models.DataBase;
using ServerPartWhatAmIToDo.Repositories.Protocols;

namespace ServerPartWhatAmIToDo.Repositories
{
    public class StepRepository : IStepRepository
    {
        private readonly DataContext _context;

        public StepRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StepEntity>> GetAllStepsAsync()
        {
            return await _context.Steps.ToListAsync();
        }

        public async Task<StepEntity> GetStepByIdAsync(int stepId)
        {
            var result = await _context.Steps.FindAsync(stepId);
            return result;
        }

        public async Task<int> AddStepAsync(StepEntity step)
        {
            await _context.Steps.AddAsync(step);
            await _context.SaveChangesAsync();
            return step.StepId;
        }

        public async Task UpdateStepAsync(StepEntity step)
        {
            _context.Steps.Update(step);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteStepAsync(int stepId)
        {
            var step = await GetStepByIdAsync(stepId);
            if (step != null)
            {
                _context.Steps.Remove(step);
                await _context.SaveChangesAsync();
            }
        }
    }
}
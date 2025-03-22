using Microsoft.EntityFrameworkCore;
using ServerPartWhatAmIToDo.Models.DataBase;
using ServerPartWhatAmIToDo.Repositories.Protocols;

namespace ServerPartWhatAmIToDo.Repositories
{
    public class DeadlineRepository : IDeadlineRepository
    {
        private readonly DataContext _context;

        public DeadlineRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DeadlineEntity>> GetAllDeadlinesAsync()
        {
            return await _context.Deadlines.ToListAsync();
        }

        public async Task<DeadlineEntity> GetDeadlineByIdAsync(int deadlineId)
        {
            return await _context.Deadlines.FindAsync(deadlineId);
        }

        public async Task<IEnumerable<DeadlineEntity>> GetDeadlinesByUserIdAsync(int userId)
        {
            return await _context.Deadlines.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<DeadlineEntity>> GetDeadlinesByGoalIdAsync(int goalId)
        {
            return await _context.Deadlines.Where(deadline => deadline.GoalId == goalId).ToListAsync();
        }

        public async Task<IEnumerable<DeadlineEntity>> GetDeadlinesByStepIdAsync(int stepId)
        {
            return await _context.Deadlines.Where(deadline => deadline.StepId == stepId).ToListAsync();
        }

        public async Task<int> AddDeadlineAsync(DeadlineEntity deadline)
        {
            await _context.Deadlines.AddAsync(deadline);
            await _context.SaveChangesAsync();
            return deadline.DeadlineId;
        }

        public async Task UpdateDeadlineAsync(DeadlineEntity deadline)
        {
            _context.Deadlines.Update(deadline);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDeadlineAsync(int deadlineId)
        {
            var deadline = await GetDeadlineByIdAsync(deadlineId);
            if (deadline != null)
            {
                _context.Deadlines.Remove(deadline);
                await _context.SaveChangesAsync();
            }
        }
    }
}
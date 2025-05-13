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

        public async Task<IEnumerable<DeadlineEntity>> GetAllDeadlinesAsync(CancellationToken cancellationToken)
        {
            return await _context.Deadlines.ToListAsync();
        }

        public async Task<DeadlineEntity> GetDeadlineByIdAsync(int deadlineId, CancellationToken cancellationToken)
        {
            return await _context.Deadlines.FindAsync(deadlineId);
        }

        public async Task<IEnumerable<DeadlineEntity>> GetDeadlinesByUserIdAsync(int userId, CancellationToken cancellationToken)
        {
            return await _context.Deadlines.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<DeadlineEntity>> GetDeadlinesByGoalIdAsync(int goalId, CancellationToken cancellationToken)
        {
            return await _context.Deadlines.Where(deadline => deadline.GoalId == goalId).ToListAsync();
        }

        public async Task<IEnumerable<DeadlineEntity>> GetDeadlinesByStepIdAsync(int stepId, CancellationToken cancellationToken)
        {
            return await _context.Deadlines.Where(deadline => deadline.StepId == stepId).ToListAsync();
        }

        public async Task<int> AddDeadlineAsync(DeadlineEntity deadline, CancellationToken cancellationToken)
        {
            await _context.Deadlines.AddAsync(deadline);
            await _context.SaveChangesAsync();
            return deadline.DeadlineId;
        }

        public async Task UpdateDeadlineAsync(DeadlineEntity deadline, CancellationToken cancellationToken)
        {
            _context.Deadlines.Update(deadline);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDeadlineAsync(int deadlineId, CancellationToken cancellationToken)
        {
            var deadline = await GetDeadlineByIdAsync(deadlineId, cancellationToken);
            if (deadline != null)
            {
                _context.Deadlines.Remove(deadline);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<DeadlineEntity>> GetDeadlinesForUserAsync(int userId, int maxDaysCount, CancellationToken cancellationToken)
        {
            // 1. Удаляем просроченные дедлайны (чья дата уже прошла по текущему времени)
            var expiredDeadlines = await _context.Deadlines
                .Where(d => d.UserId == userId && d.Deadline.Date < DateTime.UtcNow.Date)
                .ToListAsync();
            
            if (expiredDeadlines.Any())
            {
                _context.Deadlines.RemoveRange(expiredDeadlines);
                await _context.SaveChangesAsync();
            }
            
            // 2. Получаем актуальные дедлайны в пределах нужного диапазона дат
            return await _context.Deadlines
                .Where(d => d.UserId == userId 
                            && 
                            (d.Deadline.Date == DateTime.UtcNow.AddDays(maxDaysCount).Date || 
                             d.Deadline.Date == DateTime.UtcNow.Date
                             )
                            )
                .ToListAsync();
        }
    }
}
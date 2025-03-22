using Microsoft.EntityFrameworkCore;
using ServerPartWhatAmIToDo.Models.DataBase;
using ServerPartWhatAmIToDo.Repositories.Protocols;

namespace ServerPartWhatAmIToDo.Repositories
{
    public class GoalRepository : IGoalRepository
    {
        private readonly DataContext _context;
        private readonly IStepRepository _stepRepository;

        public GoalRepository(DataContext context, IStepRepository stepRepository)
        {
            _context = context;
            _stepRepository = stepRepository;
        }

        public async Task<IEnumerable<GoalEntity>> GetAllGoalsAsync()
        {
            return await _context.Goals.ToListAsync();
        }

        public async Task<GoalEntity> GetGoalByIdAsync(int goalId)
        {
            
            var result = await _context.Goals.FindAsync(goalId);
            if (result == null)
            {
                throw new KeyNotFoundException($"Goal with id {goalId} not found");
            }
            
            return result;
        }

        public async Task<IEnumerable<GoalEntity>> GetGoalsByUserIdAsync(int userId)
        {
            return await _context.Goals.Where(goal => goal.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<StepEntity>> GetStepsByGoalIdAsync(int goalId)
        {
            // Получите цель и проверьте, что она существует
            var goal = await GetGoalByIdAsync(goalId);
            if (goal == null)
            {
                throw new Exception("Goal not found");
            }

            // Получение всех шагов, соответствующих списку IdSteps
            var steps = new List<StepEntity>();
            foreach (var stepId in goal.IdSteps)
            {
                var step = await _stepRepository.GetStepByIdAsync(stepId);
                if (step != null)
                {
                    steps.Add(step);
                }
            }

            return steps;
        }

        public async Task<int> AddGoalAsync(GoalEntity goal)
        {
            await _context.Goals.AddAsync(goal);
            await _context.SaveChangesAsync();
            return goal.GoalId;
        }

        public async Task UpdateGoalAsync(GoalEntity goal)
        {
            _context.Goals.Update(goal);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteGoalAsync(int goalId)
        {
            var goal = await GetGoalByIdAsync(goalId);
            
            if (goal != null)
            {
                foreach (var step in goal.IdSteps)
                {
                    await _stepRepository.DeleteStepAsync(step);
                }
                
                _context.Goals.Remove(goal);
                await _context.SaveChangesAsync();
            }
        }
    }
}
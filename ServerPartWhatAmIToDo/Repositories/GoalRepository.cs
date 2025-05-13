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

        public async Task<IEnumerable<GoalEntity>> GetAllGoalsAsync(CancellationToken cancellationToken)
        {
            return await _context.Goals.ToListAsync();
        }

        public async Task<GoalEntity> GetGoalByIdAsync(int goalId, CancellationToken cancellationToken)
        {
            
            var result = await _context.Goals.FindAsync(goalId);
            if (result == null)
            {
                throw new KeyNotFoundException($"Goal with id {goalId} not found");
            }
            
            return result;
        }

        public async Task<IEnumerable<GoalEntity>> GetGoalsByUserIdAsync(int userId, CancellationToken cancellationToken)
        {
            var goals = await _context.Goals.Where(goal => goal.UserId == userId).ToListAsync();
            goals.Sort((x, y) => x.GoalId.CompareTo(y.GoalId));
            return goals;
        }

        public async Task<IEnumerable<StepEntity>> GetStepsByGoalIdAsync(int goalId, CancellationToken cancellationToken)
        {
            // Получите цель и проверьте, что она существует
            var goal = await GetGoalByIdAsync(goalId, cancellationToken);
            if (goal == null)
            {
                throw new Exception("Goal not found");
            }

            // Получение всех шагов, соответствующих списку IdSteps
            var steps = new List<StepEntity>();
            foreach (var stepId in goal.IdSteps)
            {
                var step = await _stepRepository.GetStepByIdAsync(stepId, cancellationToken);
                if (step != null)
                {
                    steps.Add(step);
                }
            }
            
            steps.Sort((x, y) => x.StepId.CompareTo(y.StepId));
            return steps;
        }

        public async Task<int> AddGoalAsync(GoalEntity goal, CancellationToken cancellationToken)
        {
            await _context.Goals.AddAsync(goal);
            await _context.SaveChangesAsync();
            return goal.GoalId;
        }

        public async Task UpdateGoalAsync(GoalEntity goal, CancellationToken cancellationToken)
        {
            _context.Goals.Update(goal);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteGoalAsync(int goalId, CancellationToken cancellationToken)
        {
            var goal = await GetGoalByIdAsync(goalId, cancellationToken);
            
            if (goal != null)
            {
                foreach (var step in goal.IdSteps)
                {
                    await _stepRepository.DeleteStepAsync(step, cancellationToken);
                }
                
                _context.Goals.Remove(goal);
                await _context.SaveChangesAsync();
            }
        }
    }
}
using ServerPartWhatAmIToDo.Models;
using ServerPartWhatAmIToDo.Models.DataBase;
using ServerPartWhatAmIToDo.Models.Goals;
using ServerPartWhatAmIToDo.Repositories.Protocols;

namespace ServerPartWhatAmIToDo.Services
{
    public interface IGoalService
    {
        Task<IEnumerable<GoalEntity>> GetAllGoalsAsync();
        Task<GoalEntity> GetGoalByIdAsync(int goalId);
        Task<IEnumerable<Goal>> GetGoalsByUserIdAsync(int userId);
        Task AddGoalAsync(GoalRequest goal);
        Task UpdateGoalAsync(int goalId, GoalRequest request);
        Task DeleteGoalAsync(int goalId);
    }
    
    public class GoalService : IGoalService
    {
        private readonly IGoalRepository _goalRepository;
        private readonly IStepService _stepService;
        private readonly IDeadlineService _deadlineService;
        private readonly IFilterService _filterService;

        public GoalService
        (
            IGoalRepository goalRepository,
            IStepService stepService,
            IDeadlineService deadlineService,
            IFilterService filterService
            )
        {
            _goalRepository = goalRepository;
            _stepService = stepService;
            _deadlineService = deadlineService;
            _filterService = filterService;
        }

        public async Task<IEnumerable<GoalEntity>> GetAllGoalsAsync()
        {
            return await _goalRepository.GetAllGoalsAsync();
        }

        public async Task<GoalEntity> GetGoalByIdAsync(int goalId)
        {
            return await _goalRepository.GetGoalByIdAsync(goalId);
        }

        public async Task<IEnumerable<Goal>> GetGoalsByUserIdAsync(int userId)
        {
            var results = new List<Goal>();
            var goals = await _goalRepository.GetGoalsByUserIdAsync(userId);
            foreach (var goal in goals)
            {
                var steps = await _goalRepository.GetStepsByGoalIdAsync(goal.GoalId);
                var goalRes = new Goal(goal, steps);
                
                results.Add(goalRes);
            }
            
            return results;
        }

        public async Task AddGoalAsync(GoalRequest goal)
        {
            var stepsIds = new List<int>();
            foreach (var step in goal.Steps)
            {
                var id = await _stepService.AddStepAsync(new Step(step));
                if (step.Deadline.HasValue)
                {
                    var deadline = new DeadlineEntity();
                    deadline.UserId = goal.UserId;
                    deadline.StepId = id;
                    deadline.Deadline = step.Deadline.Value;
                    await _deadlineService.AddDeadlineAsync(deadline);
                }

                stepsIds.Add(id);
            }
            var entity = new GoalEntity();
            entity.UserId = goal.UserId;
            entity.IdFilters = goal.CategoriesId.Where(id => _filterService.DoesIdExist(id)).ToArray();
            entity.StartDate = goal.StartDate;
            entity.Deadline = goal.Deadline;
            entity.IdSteps = stepsIds.ToArray();
            entity.Title = goal.Title;
            
            var goalId = await _goalRepository.AddGoalAsync(entity);
            
            if (goal.Deadline.HasValue)
            {
                var deadline = new DeadlineEntity();
                deadline.UserId = goal.UserId;
                deadline.GoalId = goalId;
                deadline.Deadline = goal.Deadline.Value;
                await _deadlineService.AddDeadlineAsync(deadline);
            }
        }

        public async Task UpdateGoalAsync(int goalId, GoalRequest request)
        {
            // Получить текущую цель из базы данных
            var currentGoal = await _goalRepository.GetGoalByIdAsync(goalId);

            if (currentGoal == null)
            {
                throw new Exception("Goal not found");
            }
            
            var stepsIds = new List<int>();

            // Получить текущие шаги, связанные с целью
            var currentSteps = await _goalRepository.GetStepsByGoalIdAsync(goalId);

            // Постоянный актуальный список шагов из модели
            var updatedSteps = request.Steps;

            // Найти шаги, которые нужно добавить
            var stepsToAdd = updatedSteps
                .Where(us => currentSteps.All(cs => cs.StepId != us.StepId))
                .ToList();

            // Найти шаги, которые нужно обновить
            var stepsToUpdate = updatedSteps
                .Where(us => currentSteps.Any(cs => cs.StepId == us.StepId))
                .ToList();

            // Найти шаги, которые нужно удалить
            var stepsToRemove = currentSteps
                .Where(cs => updatedSteps.All(us => us.StepId != cs.StepId))
                .ToList();

            // Удалить шаги
            foreach (var step in stepsToRemove)
            {
                await _stepService.DeleteStepAsync(step.StepId);
                var deadlines = await _deadlineService.GetDeadlinesByStepIdAsync(step.StepId);
                foreach (var deadline in deadlines)
                {
                    await _deadlineService.DeleteDeadlineAsync(deadline.DeadlineId);
                }
            }

            // Обновить шаги
            foreach (var step in stepsToUpdate)
            {
                var existingStep = currentSteps.First(cs => cs.StepId == step.StepId);
                existingStep.Title = step.Title;
                existingStep.IsCompleted = step.IsCompleted;
                if (existingStep.Deadline != step.Deadline && existingStep.Deadline.HasValue)
                {
                    var deadlines = await _deadlineService.GetDeadlinesByStepIdAsync(existingStep.StepId);
                    foreach (var deadline in deadlines)
                    {
                        deadline.Deadline = existingStep.Deadline.Value;
                        
                        await _deadlineService.UpdateDeadlineAsync(deadline);
                    }
                }
                existingStep.Deadline = step.Deadline;

                await  _stepService.UpdateStepAsync(existingStep);
                stepsIds.Add(existingStep.StepId);
            }

            // Добавить новые шаги
            foreach (var step in stepsToAdd)
            {
                var newStep = new Step(step);

                var id = await _stepService.AddStepAsync(newStep);
                
                if (step.Deadline.HasValue)
                {
                    var deadline = new DeadlineEntity();
                    deadline.UserId = request.UserId;
                    deadline.StepId = id;
                    deadline.Deadline = step.Deadline.Value;
                    await _deadlineService.AddDeadlineAsync(deadline);
                }
                
                stepsIds.Add(id);
            }
            
            currentGoal.Title = request.Title;
            currentGoal.Deadline = request.Deadline;
            currentGoal.StartDate = request.StartDate;
            currentGoal.IdFilters = request.CategoriesId.Where(id => _filterService.DoesIdExist(id)).ToArray();
            currentGoal.IdSteps = stepsIds.ToArray();
            await _goalRepository.UpdateGoalAsync(currentGoal);
        }

        public async Task DeleteGoalAsync(int goalId)
        {
            var steps = await _goalRepository.GetStepsByGoalIdAsync(goalId);
            foreach (var step in steps)
            {
                await _stepService.DeleteStepAsync(step.StepId);
                var deadlines = await _deadlineService.GetDeadlinesByStepIdAsync(step.StepId);
                foreach (var deadline in deadlines)
                {
                    await _deadlineService.DeleteDeadlineAsync(deadline.DeadlineId);
                }
            }
            
            var deadlinesGoals = await _deadlineService.GetDeadlinesByGoalIdAsync(goalId);
            foreach (var deadline in deadlinesGoals)
            {
                await _deadlineService.DeleteDeadlineAsync(deadline.DeadlineId);
            }
            await _goalRepository.DeleteGoalAsync(goalId);
        }
    }
}

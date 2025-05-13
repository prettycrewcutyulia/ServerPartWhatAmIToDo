using ServerPartWhatAmIToDo.Models;
using ServerPartWhatAmIToDo.Models.DataBase;
using ServerPartWhatAmIToDo.Models.Goals;
using ServerPartWhatAmIToDo.Repositories.Protocols;

namespace ServerPartWhatAmIToDo.Services
{
    public interface IGoalService
    {
        Task<IEnumerable<GoalEntity>> GetAllGoalsAsync(CancellationToken token);
        Task<GoalEntity> GetGoalByIdAsync(int goalId, CancellationToken token);
        Task<IEnumerable<Goal>> GetGoalsByUserIdAsync(int userId, CancellationToken token);
        Task AddGoalAsync(GoalRequest goal, CancellationToken token);
        Task UpdateGoalAsync(int goalId, GoalRequest request, CancellationToken token);
        Task DeleteGoalAsync(int goalId, CancellationToken token);
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

        public async Task<IEnumerable<GoalEntity>> GetAllGoalsAsync(CancellationToken token)
        {
            return await _goalRepository.GetAllGoalsAsync(token);
        }

        public async Task<GoalEntity> GetGoalByIdAsync(int goalId, CancellationToken token)
        {
            return await _goalRepository.GetGoalByIdAsync(goalId, token);
        }

        public async Task<IEnumerable<Goal>> GetGoalsByUserIdAsync(int userId, CancellationToken token)
        {
            var results = new List<Goal>();
            var goals = await _goalRepository.GetGoalsByUserIdAsync(userId, token);
            foreach (var goal in goals)
            {
                var steps = await _goalRepository.GetStepsByGoalIdAsync(goal.GoalId, token);
                var goalRes = new Goal(goal, steps);
                
                results.Add(goalRes);
            }
            
            return results;
        }

        public async Task AddGoalAsync(GoalRequest goal, CancellationToken token)
        {
            var stepsIds = new List<int>();
            foreach (var step in goal.Steps)
            {
                var id = await _stepService.AddStepAsync(new Step(step), token);
                if (step.Deadline.HasValue)
                {
                    var deadline = new DeadlineEntity();
                    deadline.UserId = goal.UserId;
                    deadline.StepId = id;
                    deadline.Deadline = step.Deadline.Value;
                    await _deadlineService.AddDeadlineAsync(deadline, token);
                }

                stepsIds.Add(id);
            }
            var entity = new GoalEntity();
            entity.UserId = goal.UserId;
            entity.IdFilters = goal.CategoriesId.Where(id => _filterService.DoesIdExist(id, token)).ToArray();
            entity.StartDate = goal.StartDate;
            entity.Deadline = goal.Deadline;
            entity.IdSteps = stepsIds.ToArray();
            entity.Title = goal.Title;
            
            var goalId = await _goalRepository.AddGoalAsync(entity, token);
            
            if (goal.Deadline.HasValue)
            {
                var deadline = new DeadlineEntity();
                deadline.UserId = goal.UserId;
                deadline.GoalId = goalId;
                deadline.Deadline = goal.Deadline.Value;
                await _deadlineService.AddDeadlineAsync(deadline, token);
            }
        }

        public async Task UpdateGoalAsync(int goalId, GoalRequest request, CancellationToken token)
        {
            // Получить текущую цель из базы данных
            var currentGoal = await _goalRepository.GetGoalByIdAsync(goalId, token);

            if (currentGoal == null)
            {
                throw new Exception("Goal not found");
            }
            
            var stepsIds = new List<int>();

            // Получить текущие шаги, связанные с целью
            var currentSteps = await _goalRepository.GetStepsByGoalIdAsync(goalId, token);

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
                await _stepService.DeleteStepAsync(step.StepId, token);
                var deadlines = await _deadlineService.GetDeadlinesByStepIdAsync(step.StepId, token);
                foreach (var deadline in deadlines)
                {
                    await _deadlineService.DeleteDeadlineAsync(deadline.DeadlineId, token);
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
                    var deadlines = await _deadlineService.GetDeadlinesByStepIdAsync(existingStep.StepId, token);
                    foreach (var deadline in deadlines)
                    {
                        deadline.Deadline = existingStep.Deadline.Value;
                        
                        await _deadlineService.UpdateDeadlineAsync(deadline, token);
                    }
                }
                existingStep.Deadline = step.Deadline;

                await  _stepService.UpdateStepAsync(existingStep, token);
                stepsIds.Add(existingStep.StepId);
            }

            // Добавить новые шаги
            foreach (var step in stepsToAdd)
            {
                var newStep = new Step(step);

                var id = await _stepService.AddStepAsync(newStep, token);
                
                if (step.Deadline.HasValue)
                {
                    var deadline = new DeadlineEntity();
                    deadline.UserId = request.UserId;
                    deadline.StepId = id;
                    deadline.Deadline = step.Deadline.Value;
                    await _deadlineService.AddDeadlineAsync(deadline, token);
                }
                
                stepsIds.Add(id);
            }
            
            currentGoal.Title = request.Title;
            currentGoal.Deadline = request.Deadline;
            currentGoal.StartDate = request.StartDate;
            currentGoal.IdFilters = request.CategoriesId.Where(id => _filterService.DoesIdExist(id, token)).ToArray();
            currentGoal.IdSteps = stepsIds.ToArray();
            await _goalRepository.UpdateGoalAsync(currentGoal, token);
        }

        public async Task DeleteGoalAsync(int goalId, CancellationToken token)
        {
            var steps = await _goalRepository.GetStepsByGoalIdAsync(goalId, token);
            foreach (var step in steps)
            {
                await _stepService.DeleteStepAsync(step.StepId, token);
                var deadlines = await _deadlineService.GetDeadlinesByStepIdAsync(step.StepId, token);
                foreach (var deadline in deadlines)
                {
                    await _deadlineService.DeleteDeadlineAsync(deadline.DeadlineId, token);
                }
            }
            
            var deadlinesGoals = await _deadlineService.GetDeadlinesByGoalIdAsync(goalId, token);
            foreach (var deadline in deadlinesGoals)
            {
                await _deadlineService.DeleteDeadlineAsync(deadline.DeadlineId, token);
            }
            await _goalRepository.DeleteGoalAsync(goalId, token);
        }
    }
}

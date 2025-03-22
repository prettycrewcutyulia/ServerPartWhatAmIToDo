
using System.Text;
using System.Text.Json;
using ServerPartWhatAmIToDo.Models;
using ServerPartWhatAmIToDo.Models.DataBase;
using ServerPartWhatAmIToDo.Repositories.Protocols;

namespace ServerPartWhatAmIToDo.Services
{
    public interface IReminderService
    {
        Task<IEnumerable<ReminderEntity>> GetAllRemindersAsync();
        Task<ReminderEntity> GetReminderByIdAsync(int reminderId);
        Task<IEnumerable<ReminderEntity>> GetRemindersByUserIdAsync(int userId);
        Task<int> AddReminderAsync(ReminderRequest reminder);
        Task DeleteReminderAsync(int reminderId);
        Task ProcessRemindersAsync();

    }
    
    public class ReminderService : IReminderService
    {
        private readonly IReminderRepository _reminderRepository;
        private readonly IDeadlineService _deadlineService;
        private readonly IGoalService _goalService;
        private readonly IStepService _stepService;
        private readonly IUserService _userService;

        public ReminderService
        (
            IReminderRepository reminderRepository,
            IDeadlineService deadlineService,
            IGoalService goalService,
            IStepService stepService,
            IUserService userService
            )
        {
            _reminderRepository = reminderRepository;
            _deadlineService = deadlineService;
            _goalService = goalService;
            _stepService = stepService;
            _userService = userService;
            
            Console.WriteLine("Deadline reminder service started.");
        }

        public async Task<IEnumerable<ReminderEntity>> GetAllRemindersAsync()
        {
            return await _reminderRepository.GetAllRemindersAsync();
        }

        public async Task<ReminderEntity> GetReminderByIdAsync(int reminderId)
        {
            return await _reminderRepository.GetReminderByIdAsync(reminderId);
        }

        public async Task<IEnumerable<ReminderEntity>> GetRemindersByUserIdAsync(int userId)
        {
            return await _reminderRepository.GetRemindersByUserIdAsync(userId);
        }

        public async Task<int> AddReminderAsync(ReminderRequest reminder)
        {
            var reminderEntity = new ReminderEntity();
            reminderEntity.UserId = reminder.UserId;
            reminderEntity.DaysCount = reminder.CountDays;
            return await _reminderRepository.AddReminderAsync(reminderEntity);
        }

        public async Task DeleteReminderAsync(int reminderId)
        {
            await _reminderRepository.DeleteReminderAsync(reminderId);
        }
        
        
        
        public async Task ProcessRemindersAsync()
        {
            try
            {
                var reminders = await _reminderRepository.GetAllRemindersAsync();
                // Группировка напоминаний по `userId`
                var groupedReminders = reminders
                    .GroupBy(r => r.UserId)
                    .Select(g => new
                    {
                        UserId = g.Key,
                        MaxDaysCount = g.Max(r => r.DaysCount)
                    });
                foreach (var reminder in groupedReminders)
                {
                    var userDeadlines = await _deadlineService.GetDeadlinesForUserAsync(reminder.UserId, reminder.MaxDaysCount);
                    
                    foreach (var deadline in userDeadlines)
                    {
                        var userReminder = await _reminderRepository.GetRemindersByUserIdAsync(deadline.UserId);
                        foreach (var reminderEntity in userReminder)
                        {
                            if (ShouldNotify(deadline, reminderEntity.DaysCount))
                            {
                                NotifyUser(deadline.UserId, deadline.GoalId, deadline.StepId);

                                break; // Выходим из внутреннего цикла, если уведомление отправлено
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in the reminder service: {ex.Message}");
            }
        }

        private bool ShouldNotify(DeadlineEntity deadline, int daysCount)
        {
            DateTime notificationDate = deadline.Deadline.AddDays(-daysCount);
            return DateTime.Now.Date >= notificationDate;
        }

        private void NotifyUser(int userId, int? goalId, int? stepId)
        {
            var user = _userService.GetUserByIdAsync(userId).Result;
            if (goalId.HasValue)
            {
                var goal = _goalService.GetGoalByIdAsync(goalId.Value).Result;
                if (goal.Deadline.HasValue)
                {
                    var reminder =
                        $"Deadline for goal {goal.Title} is at {goal.Deadline.Value.Date:yyyy-MM-dd}\n " +
                        $"Дедлайн для цели {goal.Title} {goal.Deadline.Value.Date:dd-MM-yyyy}";
                    if (user.IdTg.HasValue)
                    {
                        SendMessageToBot(new SendMessageRequest(user.IdTg.Value, reminder));
                    }
                }
            }
            
            if (stepId.HasValue)
            {
                var step = _stepService.GetStepByIdAsync(stepId.Value).Result;
                if (step.Deadline.HasValue)
                {
                    var reminder =
                        $"Deadline for step {step.Title} is at {step.Deadline.Value:yyyy-MM-dd}\n " +
                        $"Дедлайн для шага {step.Title} {step.Deadline.Value.Date:dd-MM-yyyy}";
                    if (user.IdTg.HasValue)
                    {
                        SendMessageToBot(new SendMessageRequest(user.IdTg.Value, reminder));
                    }
                }
            }
            // Реализуйте логику уведомления здесь, например, отправка email или push-уведомления.
            Console.WriteLine($"Notify user {userId} about deadline for goal {goalId}, step {stepId}");
        }

        private async void SendMessageToBot(SendMessageRequest request)
        {
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpClient httpClient = new();
            var response = await httpClient.PostAsync($"{Environment.GetEnvironmentVariable("BOT_URL")}/send-message", content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Message sent successfully.");
            }
            else
            {
                Console.WriteLine($"Failed to send message. Status code: {response.StatusCode}");
            }
        }
    }
}

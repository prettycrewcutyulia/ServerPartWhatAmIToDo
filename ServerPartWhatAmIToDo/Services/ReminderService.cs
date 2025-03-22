
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
    }
    
    public class ReminderService : IReminderService
    {
        private readonly IReminderRepository _reminderRepository;

        public ReminderService(IReminderRepository reminderRepository)
        {
            _reminderRepository = reminderRepository;
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
    }
}

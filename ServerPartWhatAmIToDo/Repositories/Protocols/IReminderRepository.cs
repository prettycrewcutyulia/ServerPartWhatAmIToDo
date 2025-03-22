using ServerPartWhatAmIToDo.Models.DataBase;

namespace ServerPartWhatAmIToDo.Repositories.Protocols
{
    public interface IReminderRepository
    {
        Task<IEnumerable<ReminderEntity>> GetAllRemindersAsync();
        Task<ReminderEntity> GetReminderByIdAsync(int reminderId);
        Task<IEnumerable<ReminderEntity>> GetRemindersByUserIdAsync(int userId);
        Task<int> AddReminderAsync(ReminderEntity reminder);
        Task UpdateReminderAsync(ReminderEntity reminder);
        Task DeleteReminderAsync(int reminderId);
    }
}
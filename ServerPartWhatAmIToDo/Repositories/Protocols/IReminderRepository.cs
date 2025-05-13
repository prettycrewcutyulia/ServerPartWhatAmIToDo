using ServerPartWhatAmIToDo.Models.DataBase;

namespace ServerPartWhatAmIToDo.Repositories.Protocols
{
    public interface IReminderRepository
    {
        Task<IEnumerable<ReminderEntity>> GetAllRemindersAsync(CancellationToken cancellationToken);
        Task<ReminderEntity> GetReminderByIdAsync(int reminderId, CancellationToken cancellationToken);
        Task<IEnumerable<ReminderEntity>> GetRemindersByUserIdAsync(int userId, CancellationToken cancellationToken);
        Task<int> AddReminderAsync(ReminderEntity reminder, CancellationToken cancellationToken);
        Task UpdateReminderAsync(ReminderEntity reminder, CancellationToken cancellationToken);
        Task DeleteReminderAsync(int reminderId, CancellationToken cancellationToken);
    }
}
using Microsoft.EntityFrameworkCore;
using ServerPartWhatAmIToDo.Models.DataBase;
using ServerPartWhatAmIToDo.Repositories.Protocols;

namespace ServerPartWhatAmIToDo.Repositories
{
    public class ReminderRepository : IReminderRepository
    {
        private readonly DataContext _context;

        public ReminderRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ReminderEntity>> GetAllRemindersAsync()
        {
            return await _context.Reminders.ToListAsync();
        }

        public async Task<ReminderEntity> GetReminderByIdAsync(int reminderId)
        {
            return await _context.Reminders.FindAsync(reminderId);
        }

        public async Task<IEnumerable<ReminderEntity>> GetRemindersByUserIdAsync(int userId)
        {
            return await _context.Reminders.Where(reminder => reminder.UserId == userId).ToListAsync();
        }

        public async Task<int> AddReminderAsync(ReminderEntity reminder)
        {
            await _context.Reminders.AddAsync(reminder);
            await _context.SaveChangesAsync();
            return reminder.ReminderId;
        }

        public async Task UpdateReminderAsync(ReminderEntity reminder)
        {
            _context.Reminders.Update(reminder);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteReminderAsync(int reminderId)
        {
            var reminder = await GetReminderByIdAsync(reminderId);
            if (reminder != null)
            {
                _context.Reminders.Remove(reminder);
                await _context.SaveChangesAsync();
            }
        }
    }
}
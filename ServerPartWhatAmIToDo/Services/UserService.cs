using System.Security.AccessControl;
using ServerPartWhatAmIToDo.Models;
using ServerPartWhatAmIToDo.Models.DataBase;
using ServerPartWhatAmIToDo.Models.Goals;
using ServerPartWhatAmIToDo.Repositories.Protocols;

namespace ServerPartWhatAmIToDo.Services
{
    public interface IUserService
    {
        Task<(bool, LoginResponse?)> Login(string email, string password);
        Task<IEnumerable<UserEntity>> GetAllUsersAsync();
        Task<UserEntity?> GetUserByIdAsync(int userId);
        Task<UserEntity?> GetUserByEmailAsync(string email);
        Task<long> AddUserAsync(RegisterRequest user);
        Task<bool> ExistTgUserAsync(int userId);
        Task UpdateUserAsync(UpdateAccountRequest user);
        
        Task UpdateUserAsync(UpdateTgRequest user);
        Task DeleteUserAsync(int userId);
    }
    
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IDeadlineRepository _deadlineRepository;
        private readonly IReminderRepository _reminderRepository;
        private readonly IGoalRepository _goalRepository;
        private readonly IFilterRepository _filterRepository;

        public UserService(
            IUserRepository userRepository,
            IDeadlineRepository deadlineRepository,
            IReminderRepository reminderRepository,
            IGoalRepository goalRepository,
            IFilterRepository filterRepository)
        {
            _userRepository = userRepository;
            _deadlineRepository = deadlineRepository;
            _reminderRepository = reminderRepository;
            _goalRepository = goalRepository;
            _filterRepository = filterRepository;
        }

        public async Task<(bool, LoginResponse?)> Login(string email, string password)
        {
            try
            {
                var user = await _userRepository.GetUserByEmailAsync(email);
                if (PasswordService.VerifyPassword(password, user.Password))
                {
                    var tokenString = TokenService.GenerateToken(email).Result;
                    var loginResponse = new LoginResponse(user.UserId, user.Name, user.Email, tokenString);
                    return (true, loginResponse);
                }
            }
            catch
            {
                return (false, null);
            }
            
            return (false, null);
        }

        public async Task<IEnumerable<UserEntity>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<UserEntity?> GetUserByIdAsync(int userId)
        {
            return await _userRepository.GetUserByIdAsync(userId);
        }
        
        public async Task<UserEntity?> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetUserByEmailAsync(email);
        }

        public async Task<long> AddUserAsync(RegisterRequest user)
        {
            User userData = new User(
                user.Nickname,
                user.Email,
               PasswordService.HashPassword(user.Password)
            );
            // Здесь можно добавить валидацию или другую бизнес-логику
            return await _userRepository.AddUserAsync(userData);
        }

        public async Task<bool> ExistTgUserAsync(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            return user.IdTg != null;
        }

        public async Task UpdateUserAsync(UpdateAccountRequest user)
        {
            
            var userEntity = await _userRepository.GetUserByIdAsync(user.UserId);
            if (user.Name != null)
            {
                userEntity.Name = user.Name;
            }

            if (user.Email != null)
            {
                userEntity.Email = user.Email;
            }
            // Здесь можно добавить проверку существования пользователя и другую бизнес-логику
            await _userRepository.UpdateUserAsync(userEntity);
        }

        public async Task UpdateUserAsync(UpdateTgRequest user)
        {
            var userEntity = await _userRepository.GetUserByEmailAsync(user.Email);
            userEntity.IdTg = user.TgId;
            await _userRepository.UpdateUserAsync(userEntity);
        }

        public async Task DeleteUserAsync(int userId)
        {
            var reminders = await _reminderRepository.GetRemindersByUserIdAsync(userId);
            foreach (var reminder in reminders)
            {
                await _reminderRepository.DeleteReminderAsync(reminder.ReminderId);
            }
            
            var deadlines = await _deadlineRepository.GetDeadlinesByUserIdAsync(userId);
            foreach (var deadline in deadlines)
            {
                await _deadlineRepository.DeleteDeadlineAsync(deadline.DeadlineId);
            }
            
            var goals = await _goalRepository.GetGoalsByUserIdAsync(userId);
            foreach (var goal in goals)
            {
                await _goalRepository.DeleteGoalAsync(goal.GoalId);
            }
            
            var filters = await _filterRepository.GetFiltersByUserIdAsync(userId);
            foreach (var filter in filters)
            {
                await _filterRepository.DeleteFilterAsync(filter.FilterId);
            }
            // Здесь можно добавить проверку существования пользователя или другие условия
            await _userRepository.DeleteUserAsync(userId);
        }
    }
}
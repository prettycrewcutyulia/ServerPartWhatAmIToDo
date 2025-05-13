using System.Security.AccessControl;
using ServerPartWhatAmIToDo.Models;
using ServerPartWhatAmIToDo.Models.DataBase;
using ServerPartWhatAmIToDo.Models.Goals;
using ServerPartWhatAmIToDo.Repositories.Protocols;

namespace ServerPartWhatAmIToDo.Services
{
    public interface IUserService
    {
        Task<(bool, LoginResponse?)> Login(string email, string password, CancellationToken cancellationToken);
        Task<IEnumerable<UserEntity>> GetAllUsersAsync(CancellationToken cancellationToken);
        Task<UserEntity?> GetUserByIdAsync(int userId, CancellationToken cancellationToken);
        Task<UserEntity?> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
        Task<long> AddUserAsync(RegisterRequest user, CancellationToken cancellationToken);
        Task<bool> ExistTgUserAsync(int userId, CancellationToken cancellationToken);
        Task UpdateUserAsync(UpdateAccountRequest user, CancellationToken cancellationToken);
        
        Task UpdateUserAsync(UpdateTgRequest user, CancellationToken cancellationToken);
        Task DeleteUserAsync(int userId, CancellationToken cancellationToken);
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

        public async Task<(bool, LoginResponse?)> Login(string email, string password, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.GetUserByEmailAsync(email, cancellationToken);
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

        public async Task<IEnumerable<UserEntity>> GetAllUsersAsync(CancellationToken cancellationToken)
        {
            return await _userRepository.GetAllUsersAsync(cancellationToken);
        }

        public async Task<UserEntity?> GetUserByIdAsync(int userId, CancellationToken cancellationToken)
        {
            return await _userRepository.GetUserByIdAsync(userId, cancellationToken);
        }
        
        public async Task<UserEntity?> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _userRepository.GetUserByEmailAsync(email, cancellationToken);
        }

        public async Task<long> AddUserAsync(RegisterRequest user, CancellationToken cancellationToken)
        {
            User userData = new User(
                user.Nickname,
                user.Email,
               PasswordService.HashPassword(user.Password)
            );
            return await _userRepository.AddUserAsync(userData, cancellationToken);
        }

        public async Task<bool> ExistTgUserAsync(int userId, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(userId, cancellationToken);
            return user.IdTg != null;
        }

        public async Task UpdateUserAsync(UpdateAccountRequest user, CancellationToken cancellationToken)
        {
            
            var userEntity = await _userRepository.GetUserByIdAsync(user.UserId, cancellationToken);
            if (user.Name != null)
            {
                userEntity.Name = user.Name;
            }

            if (user.Email != null)
            {
                userEntity.Email = user.Email;
            }

            await _userRepository.UpdateUserAsync(userEntity, cancellationToken);
        }

        public async Task UpdateUserAsync(UpdateTgRequest user, CancellationToken cancellationToken)
        {
            var userEntity = await _userRepository.GetUserByEmailAsync(user.Email, cancellationToken);
            userEntity.IdTg = user.TgId;
            await _userRepository.UpdateUserAsync(userEntity, cancellationToken);
        }

        public async Task DeleteUserAsync(int userId, CancellationToken cancellationToken)
        {
            var reminders = await _reminderRepository.GetRemindersByUserIdAsync(userId, cancellationToken);
            foreach (var reminder in reminders)
            {
                await _reminderRepository.DeleteReminderAsync(reminder.ReminderId, cancellationToken);
            }
            
            var deadlines = await _deadlineRepository.GetDeadlinesByUserIdAsync(userId, cancellationToken);
            foreach (var deadline in deadlines)
            {
                await _deadlineRepository.DeleteDeadlineAsync(deadline.DeadlineId, cancellationToken);
            }
            
            var goals = await _goalRepository.GetGoalsByUserIdAsync(userId, cancellationToken);
            foreach (var goal in goals)
            {
                await _goalRepository.DeleteGoalAsync(goal.GoalId, cancellationToken);
            }
            
            var filters = await _filterRepository.GetFiltersByUserIdAsync(userId, cancellationToken);
            foreach (var filter in filters)
            {
                await _filterRepository.DeleteFilterAsync(filter.FilterId, cancellationToken);
            }
            await _userRepository.DeleteUserAsync(userId, cancellationToken);
        }
    }
}
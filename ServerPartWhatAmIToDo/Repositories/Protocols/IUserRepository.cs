using ServerPartWhatAmIToDo.Models;
using ServerPartWhatAmIToDo.Models.DataBase;

namespace ServerPartWhatAmIToDo.Repositories.Protocols
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserEntity>> GetAllUsersAsync();
        Task<UserEntity> GetUserByIdAsync(int userId);
        Task<UserEntity> GetUserByEmailAsync(string email);
        Task<long> AddUserAsync(User user);
        Task UpdateUserAsync(UserEntity user);
        Task DeleteUserAsync(int userId);
    }
}
using ServerPartWhatAmIToDo.Models;
using ServerPartWhatAmIToDo.Models.DataBase;

namespace ServerPartWhatAmIToDo.Repositories.Protocols
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserEntity>> GetAllUsersAsync(CancellationToken cancellationToken);
        Task<UserEntity> GetUserByIdAsync(int userId, CancellationToken cancellationToken);
        Task<UserEntity> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
        Task<long> AddUserAsync(User user, CancellationToken cancellationToken);
        Task UpdateUserAsync(UserEntity user, CancellationToken cancellationToken);
        Task DeleteUserAsync(int userId, CancellationToken cancellationToken);
    }
}
using Microsoft.EntityFrameworkCore;
using ServerPartWhatAmIToDo.Models;
using ServerPartWhatAmIToDo.Models.DataBase;
using ServerPartWhatAmIToDo.Repositories.Protocols;

namespace ServerPartWhatAmIToDo.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<UserEntity>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<UserEntity> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<UserEntity> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<long> AddUserAsync(User user)
        {
            try
            {
                UserEntity userEntity = new UserEntity
                {
                    Name = user.Name,
                    Email = user.Email,
                    Password = user.Password
                };

                _context.Users.Add(userEntity);
                await _context.SaveChangesAsync();

                return userEntity.UserId;
            }
            catch (Exception ex)
            {
                // Логирование ошибки или повторная обработка.
                Console.WriteLine("Ошибка при добавлении пользователя: " + ex.Message);
                throw;
            }
        }

        public async Task UpdateUserAsync(UserEntity user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await GetUserByIdAsync(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
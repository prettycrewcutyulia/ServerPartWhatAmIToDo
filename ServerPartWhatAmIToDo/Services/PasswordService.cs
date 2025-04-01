using BCrypt.Net;

namespace ServerPartWhatAmIToDo.Services;

public static class PasswordService
{
    // Хэширование пароля
    public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    // Проверка пароля
    public static bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}

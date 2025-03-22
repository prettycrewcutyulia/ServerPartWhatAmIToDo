using Microsoft.EntityFrameworkCore;
using ServerPartWhatAmIToDo.Repositories;
using ServerPartWhatAmIToDo.Repositories.Protocols;
using ServerPartWhatAmIToDo.Services;

namespace ServerPartWhatAmIToDo;

public static class ServiceCollectionExtensions
{
    public static void AddDataAccessServices(this IServiceCollection services, IConfiguration configuration)
    {
        DotNetEnv.Env.Load();
        // Получение строки подключения из переменных окружения
        // Либо из конфига: configuration.GetConnectionString("DefaultConnection")
        var connectionString = Environment.GetEnvironmentVariable("DefaultConnection");

        // Добавляем DbContext с использованием строки подключения
        
        services.AddDbContext<DataContext>(options =>
            options.UseNpgsql(connectionString), ServiceLifetime.Scoped);
        
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IGoalRepository, GoalRepository>();
        services.AddScoped<IStepRepository, StepRepository>();
        services.AddScoped<IFilterRepository, FilterRepository>();
        services.AddScoped<IReminderRepository, ReminderRepository>();
        services.AddScoped<IDeadlineRepository, DeadlineRepository>();
    }
    
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IGoalService, GoalService>();
        services.AddScoped<IStepService, StepService>();
        services.AddScoped<IReminderService, ReminderService>();
        services.AddScoped<IDeadlineService, DeadlineService>();
        services.AddScoped<IFilterService, FilterService>();
    }
}
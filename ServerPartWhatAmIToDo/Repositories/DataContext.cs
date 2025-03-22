using Microsoft.EntityFrameworkCore;
using ServerPartWhatAmIToDo.Models.DataBase;

namespace ServerPartWhatAmIToDo.Repositories;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    internal DbSet<UserEntity> Users { get; set; }
    internal DbSet<GoalEntity> Goals { get; set; }
    internal DbSet<StepEntity> Steps { get; set; }
    internal DbSet<FilterEntity> Filters { get; set; }
    internal DbSet<ReminderEntity> Reminders { get; set; }
    internal DbSet<DeadlineEntity> Deadlines { get; set; }
}
namespace ServerPartWhatAmIToDo.Services;


using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

public class ReminderProcessingService : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private Timer _timer;

    public ReminderProcessingService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("ReminderProcessingService is starting.");
        _timer = new Timer(ProcessReminders, null, TimeSpan.Zero, TimeSpan.FromHours(24));

        return Task.CompletedTask;
    }
    private async void ProcessReminders(object state)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var reminderService = scope.ServiceProvider.GetRequiredService<IReminderService>();
            Console.WriteLine("Processing reminders...");
            await reminderService.ProcessRemindersAsync(CancellationToken.None);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("ReminderProcessingService is stopping.");
        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}

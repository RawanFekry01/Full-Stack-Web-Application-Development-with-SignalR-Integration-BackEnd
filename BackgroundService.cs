using Microsoft.AspNetCore.SignalR;
public class ProcessService : BackgroundService
{
    private readonly IHubContext<ProgressHub> _hubContext;
    private static List<string> storedData = new List<string>();
    private static ManualResetEventSlim completedProcess = new ManualResetEventSlim(false);
    private static readonly object storedDataLock = new object();

    public ProcessService(IHubContext<ProgressHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public void StartProcess()
    {
        completedProcess.Reset();
        Task.Run(() => ExecuteProcessAsync());
    }

    private async Task ExecuteProcessAsync()
    {
        for (int i = 1; i <= 100; i++)
        {
            await Task.Delay(500);
            await _hubContext.Clients.All.SendAsync("ReceiveProgress", i);
        }

        lock (storedDataLock)
        {
            storedData.Add(Guid.NewGuid().ToString());
        }

        completedProcess.Set();
    }

    public List<string> GetData()
    {
        completedProcess.Wait();
        lock (storedDataLock)
        {
            return new List<string>(storedData);
        }
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.CompletedTask;
    }
}


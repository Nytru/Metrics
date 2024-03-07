namespace PrometheusGrafanaTry.Services;

public class HardWorkService
{
    private object _lock = new();
    private volatile bool _isWorking = true;
    private readonly SemaphoreSlim _semaphoreSlim = new (1, 1);
    private readonly ILogger<HardWorkService> _logger;

    public HardWorkService(ILogger<HardWorkService> logger)
    {
        _logger = logger;
        Stop();
    }

    public async Task Init()
    {
        StartWork();
    }

    public void Start()
    {
        if (_isWorking)
        {
            return;
        }

        lock (_lock)
        {
            _isWorking = true;
            Monitor.PulseAll(_lock);
        }
    }

    public void Stop()
    {
        if (_isWorking is false)
        {
            return;
        }

        lock (_lock)
        {
            _isWorking = false;
        }
    }

    private Task StartWork()
    {
        return Task.Factory.StartNew(() =>
        {
            while (true)
            {
                if (_isWorking is false)
                {
                    lock (_lock)
                    {
                        Monitor.Wait(_lock);
                    }
                }

                ulong j = 0;
                for (var i = 0; i < int.MaxValue; i++)
                {
                    j += (ulong)i;
                }

                _logger.LogInformation("Done again at: {time}", DateTime.Now);
            }
        });
    }
}

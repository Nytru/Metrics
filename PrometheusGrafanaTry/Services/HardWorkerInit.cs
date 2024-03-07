namespace PrometheusGrafanaTry.Services;

public class HardWorkerInit(HardWorkService hardWorkService) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await hardWorkService.Init();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}
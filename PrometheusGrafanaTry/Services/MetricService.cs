namespace PrometheusGrafanaTry.Services;

public class MetricService
{
    private readonly HttpClient _client;

    public MetricService(HttpClient client)
    {
        _client = client;
        _client.BaseAddress = new Uri("http://localhost:5213");
    }


    private static volatile int _counter = 0;
    public string GetMetrics()
    {
        var rand = Random.Shared.Next(0, 10) == 0;
        if (_counter <= 0 && rand)
        {
            Interlocked.Add(ref _counter, 10);
        }

        int cpuTemp;
        if (_counter > 0)
        {
            cpuTemp = 100;
            Interlocked.Decrement(ref _counter);
        }
        else
        {
            cpuTemp = Random.Shared.Next(40, 50);
        }

        return
            "# HELP node_disk_io_time_weighted_seconds_total The weighted # of seconds spent doing I/Os.\n" +
            "# TYPE node_disk_io_time_weighted_seconds_total counter\n" +
            "temperature_hehe_haha{target=\"CPU\"} " + $"{cpuTemp}\n" +
            "temperature_hehe_haha{target=\"GPU\"} " + $"{Random.Shared.Next(60, 70)}";
    }
}

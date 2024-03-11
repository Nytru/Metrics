using System.Diagnostics;
using PrometheusGrafanaTry.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Configuration.AddJsonFile("appsettings.json", false, true);
builder.Services.AddHostedService<HardWorkerInit>();
builder.Services.AddSingleton<HardWorkService>();
builder.Services.AddHttpClient<MetricService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/work/{state:bool}", (bool state, HardWorkService service) =>
{
    if (state)
    {
        service.Start();
    }
    else
    {
        service.Stop();
    }
});

app.MapGet("metrics", (MetricService service) => service.GetMetrics());

app.MapPost("webhook", () =>
{
    var dockerStats = ExecuteCommand("docker stats --no-stream");
    var dockerPs = ExecuteCommand("docker ps");
    using var stream = File.AppendText($"{DateTime.Today:dd-MM-yyyy}.log");
    stream.Write($"-------------------------\n{DateTime.UtcNow:O}\n");
    stream.Write(dockerStats);
    stream.Write("#########################\n");
    stream.Write(dockerPs);
});

app.Run();
return;


string ExecuteCommand(string command)
{

    var psi = new ProcessStartInfo("/bin/bash", $"-c \"{command}\"")
    {
        RedirectStandardOutput = true,
        UseShellExecute = false,
        CreateNoWindow = true
    };

    var process = new Process();
    process.StartInfo = psi;
    process.Start();

    var output = process.StandardOutput.ReadToEnd();
    process.WaitForExit();
    return output;
}

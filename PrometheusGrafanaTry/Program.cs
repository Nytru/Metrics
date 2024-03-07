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

app.MapPost("webhook/{value:int}", (int value) =>
{
    Console.WriteLine($"DateTime: {DateTime.Now}, got request");
    return value;
});

app.Run();

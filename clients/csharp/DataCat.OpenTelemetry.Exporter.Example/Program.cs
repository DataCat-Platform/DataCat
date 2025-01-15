var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<MyMetric>();

builder.Services.AddOpenTelemetry()
    .WithMetrics(c => c
        .AddMeter(MyMetric.InstrumentSourceName)
        // .AddHttpClientInstrumentation()
        // .AddAspNetCoreInstrumentation()
        // .AddRuntimeInstrumentation()
        .AddDataCatExporter(c =>
        {
            c.Address = "http://localhost:5000";
        }));
        // .AddConsoleExporter()
        // .AddOtlpExporter(opt =>
        // {
        //     // opt.BatchExportProcessorOptions = new BatchExportActivityProcessorOptions() {}
        //     opt.Protocol = OtlpExportProtocol.Grpc;
        // }))

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/weatherforecast", (MyMetric metric) =>
    {
        metric.MyCounter.Add(1, new KeyValuePair<string, object?>("weather", "today"));
        return Results.Ok("all is ok");
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.Run();


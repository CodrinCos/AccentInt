using AccentInt.Infrastructure;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

//Logger
var logger = LoggerFactory.Create(config => config.AddConsole()).CreateLogger("Program");

builder.Services.AddControllers().AddJsonOptions(opts =>
{
    opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    opts.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
});

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

builder.Services.AddInfrastructureServices(logger);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseExceptionHandler();
app.MapDefaultEndpoints();
app.MapControllers();

app.Run();

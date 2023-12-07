using Personal.Timeline.Web.Models.Js3;
using Personal.Timeline.Web.Models.Vis;
using Personal.Timeline.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json");

builder.Services.AddSingleton<IOutputWriter, OutputWriterService>();
builder.Services.AddSingleton<Js3TimelineService>();
builder.Services.AddSingleton<VisTimelineService>();
builder.Services.AddSingleton<SourceReaderService>();
builder.Services.AddSingleton<TimelineGeneratorService>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.UseStaticFiles();

// generate the timelines
var timelineGenerator = app.Services
    .GetRequiredService<TimelineGeneratorService>();
timelineGenerator.GenerateAllAsync().GetAwaiter().GetResult();

app.Run();
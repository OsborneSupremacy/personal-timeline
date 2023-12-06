using Personal.Timeline.Web.Models.Js3;
using Personal.Timeline.Web.Models.Vis;
using Personal.Timeline.Web.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IOutputWriter, OutputWriterService>();
builder.Services.AddSingleton<ITimelineGenerator<Js3Timeline>, Js3TimelineService>();
builder.Services.AddSingleton<ITimelineGenerator<VisTimeline>, VisTimelineService>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.UseStaticFiles();

app.Run();
using Personal.Timeline.Web.Models.Js3;

namespace Personal.Timeline.Web.Services;

public class Js3TimelineService : ITimelineGenerator<Js3Timeline>
{
    private readonly IOutputWriter _outputWriter;

    public Js3TimelineService(IOutputWriter outputWriter)
    {
        _outputWriter = outputWriter ?? throw new ArgumentNullException(nameof(outputWriter));
    }

    public Task<Js3Timeline> GenerateAsync(TimelineRequest request)
    {
        List<Js3Event> events = new();

        foreach(var item in request.Items)
        {
            Js3Date? endDate = null;

            if (item.EndDate.HasValue)
                endDate = new Js3Date
                {
                    Year = item.EndDate.Value.Year.ToString(),
                    Month = item.EndDate.Value.Month.ToString(),
                    Day = item.EndDate.Value.Day.ToString()
                };
            
            StringBuilder description = new();
           
            if (!string.IsNullOrEmpty(item.Description1))
                description.AppendLine($"<p>{item.Description1.Trim().Replace("\n", "<br /><br />").Replace("  ", "<br /><br />")}</p>");
            
            if (!string.IsNullOrEmpty(item.Description2))
                description.AppendLine($"<p>{item.Description2.Trim().Replace("\n", "<br /><br />").Replace("  ", "<br /><br />")}</p>");
            
            if (!string.IsNullOrEmpty(item.Url))
                description.AppendLine($"<p><a href=\"{item.Url}\">{item.UrlDescription}</a></p>");
            
            events.Add(new Js3Event
            {
                Group = item.Group,
                Text = new()
                {
                    Headline = item.Headline,
                    Text = description.ToString().Replace("\n", string.Empty)
                },
                StartDate = new Js3Date
                {
                    Year = item.StartDate.Year.ToString(),
                    Month = item.StartDate.Month.ToString(),
                    Day = item.StartDate.Day.ToString()
                },
                EndDate = endDate
            });
        }

        var timeline = new Js3Timeline
        {
            Title = new Js3Title
            {
                Media = new Js3Media
                {
                    Url = "/images/main-image.jpeg",
                    Caption = "Ben Osborne something something something."
                },
                Text = new Js3Text
                {
                    Headline = $"Ben Osborne",
                    Text = "<p>Ben Osborne something something something.</p>"
                }
            },
            Eras = new(),
            Events = events
                .OrderBy(e => e.Group)
                .ThenBy(e => e.StartDate.Year)
                .ThenBy(e => e.StartDate.Month)
                .ThenBy(e => e.StartDate.Day)
                .ToList()
        };

        return Task.FromResult(timeline);
    }

    public async Task WriteAsync(Js3Timeline data, string basePath) =>
        await _outputWriter.WriteAsync<Js3Timeline>(new()
        {
            Data = data,
            FileName = "output-timeline3.json",
            BasePath = basePath
        });
}
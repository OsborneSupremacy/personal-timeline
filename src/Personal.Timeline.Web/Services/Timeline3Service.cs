using Personal.Timeline.Web.Models.Timeline3;

namespace Personal.Timeline.Web.Services;

public class Timeline3Service : ITimelineGenerator<Timeline3Timeline>
{
    private readonly IOutputWriter _outputWriter;

    public Timeline3Service(IOutputWriter outputWriter)
    {
        _outputWriter = outputWriter ?? throw new ArgumentNullException(nameof(outputWriter));
    }

    public Task<Timeline3Timeline> GenerateAsync(TimelineRequest request)
    {
        List<Event> events = new();

        foreach(var item in request.Items)
        {
            TimelineDate? endDate = null;

            if (item.EndDate.HasValue)
                endDate = new TimelineDate
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
            
            events.Add(new Event
            {
                Group = item.Group,
                Text = new()
                {
                    Headline = item.Headline,
                    Text = description.ToString().Replace("\n", string.Empty)
                },
                StartDate = new TimelineDate
                {
                    Year = item.StartDate.Year.ToString(),
                    Month = item.StartDate.Month.ToString(),
                    Day = item.StartDate.Day.ToString()
                },
                EndDate = endDate
            });
        }

        var timeline = new Timeline3Timeline
        {
            Title = new TimelineTitle
            {
                Media = new TimelineMedia
                {
                    Url = "/images/main-image.jpeg",
                    Caption = "Ben Osborne something something something."
                },
                Text = new TimelineText
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

    public async Task WriteAsync(Timeline3Timeline data, string basePath) =>
        await _outputWriter.WriteAsync<Timeline3Timeline>(new()
        {
            Data = data,
            FileName = "output-timeline3.json",
            BasePath = basePath
        });
}
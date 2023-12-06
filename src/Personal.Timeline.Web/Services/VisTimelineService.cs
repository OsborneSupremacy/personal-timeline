using Personal.Timeline.Web.Models.VisTimeline;

namespace Personal.Timeline.Web.Services;

public class VisTimelineService : ITimelineGenerator<VisTimelineTimeline>
{
    private readonly IOutputWriter _outputWriter;

    public VisTimelineService(IOutputWriter outputWriter)
    {
        _outputWriter = outputWriter ?? throw new ArgumentNullException(nameof(outputWriter));
    }

    public async Task<VisTimelineTimeline> GenerateAsync(TimelineRequest request)
    {
        var groups = await GenerateGroupsAsync(request.Items);
        var items = await GenerateItemsAsync(request.Items, groups);
        return new VisTimelineTimeline
        {
            Groups = groups.Values.ToList(),
            Items = items
        };
    }

    public async Task WriteAsync(VisTimelineTimeline data, string basePath)
    {
        await _outputWriter.WriteAsync<List<VisTimelineGroup>>(new()
        {
            Data = data.Groups,
            FileName = "output-timeline3.json",
            BasePath = basePath
        });
        
        await _outputWriter.WriteAsync<List<VisTimelineItem>>(new()
        {
            Data = data.Items,
            FileName = "output-timeline3.json",
            BasePath = basePath
        });
    }

    private static Task<Dictionary<string, VisTimelineGroup>> GenerateGroupsAsync(
        IEnumerable<SourceItem> records
    )
    {
        var groups = records
            .Select(x => x.Group)
            .Distinct()
            .ToDictionary
            (
                g => g,
                g => new VisTimelineGroup() { Id = g, Title = g, Content = g }
            );

        return Task.FromResult(groups);
    }
    
    private static Task<List<VisTimelineItem>> GenerateItemsAsync(
        List<SourceItem> records,
        IReadOnlyDictionary<string, VisTimelineGroup> groupDictionary
    )
    {
        int id = 0;
        List<VisTimelineItem> items = new();
        
        foreach (var record in records)
        {
            StringBuilder description = new();
           
            if (!string.IsNullOrEmpty(record.Description1))
                description.AppendLine($"<p>{record.Description1.Trim().Replace("\n", "<br /><br />").Replace("  ", "<br /><br />")}</p>");
            
            if (!string.IsNullOrEmpty(record.Description2))
                description.AppendLine($"<p>{record.Description2.Trim().Replace("\n", "<br /><br />").Replace("  ", "<br /><br />")}</p>");
            
            if (!string.IsNullOrEmpty(record.Url))
                description.AppendLine($"<p><a href=\"{record.Url}\">{record.UrlDescription}</a></p>");

            var group = groupDictionary.GetValueOrDefault(record.Group);
            
            VisTimelineItem item = new()
            {
                Id = (++id).ToString(),
                Title = record.Headline ?? string.Empty,
                Type = record.StartDate == record.EndDate ? "point" : "range",
                Group = group?.Title ?? string.Empty,
                SubGroup = string.Empty,
                Content = record.Headline ?? string.Empty,
                Start = DateOnly.FromDateTime(record.StartDate),
                End = record.EndDate.HasValue ? DateOnly.FromDateTime(record.EndDate.Value) : DateOnly.FromDateTime(DateTime.Now),
                Selectable = true
            };
            items.Add(item);
        }

        return Task.FromResult(items);
    }
}
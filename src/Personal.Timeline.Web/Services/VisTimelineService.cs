using Personal.Timeline.Web.Models.Vis;

namespace Personal.Timeline.Web.Services;

public class VisTimelineService : ITimelineGenerator<VisTimeline>
{
    private readonly IOutputWriter _outputWriter;

    public VisTimelineService(IOutputWriter outputWriter)
    {
        _outputWriter = outputWriter ?? throw new ArgumentNullException(nameof(outputWriter));
    }

    public async Task<VisTimeline> GenerateAsync(TimelineRequest request)
    {
        var groups = await GenerateGroupsAsync(request.Items);
        var items = await GenerateItemsAsync(request.Items, groups);
        return new VisTimeline
        {
            Groups = groups.Values.ToList(),
            Items = items
        };
    }

    public async Task WriteAsync(VisTimeline data, string basePath)
    {
        await _outputWriter.WriteAsync<List<VisGroup>>(new()
        {
            Data = data.Groups,
            FileName = "groups.json",
            BasePath = basePath
        });
        
        await _outputWriter.WriteAsync<List<VisItem>>(new()
        {
            Data = data.Items,
            FileName = "items.json",
            BasePath = basePath
        });
    }

    private static Task<Dictionary<string, VisGroup>> GenerateGroupsAsync(
        IEnumerable<SourceItem> records
    )
    {
        var groups = records
            .Select(x => x.Group)
            .Distinct()
            .ToDictionary
            (
                g => g,
                g => new VisGroup() { Id = g, Title = g, Content = g }
            );

        return Task.FromResult(groups);
    }
    
    private static Task<List<VisItem>> GenerateItemsAsync(
        List<SourceItem> records,
        IReadOnlyDictionary<string, VisGroup> groupDictionary
    )
    {
        int id = 0;
        List<VisItem> items = new();
        
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
            
            VisItem item = new()
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
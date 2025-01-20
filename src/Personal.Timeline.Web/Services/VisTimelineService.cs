using Personal.Timeline.Web.Extensions;
using Personal.Timeline.Web.Models.Vis;

namespace Personal.Timeline.Web.Services;

internal class VisTimelineService : ITimelineGenerator<VisTimeline>
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
        IEnumerable<Occurence> records
    )
    {
        var groups = records
            .Select(x => x.Group)
            .Distinct()
            .ToDictionary
            (
                g => g,
                g => new VisGroup { Id = g, Title = g, Content = g }
            );

        return Task.FromResult(groups);
    }
    
    private static Task<List<VisItem>> GenerateItemsAsync(
        List<Occurence> records,
        IReadOnlyDictionary<string, VisGroup> groupDictionary
    )
    {
        var id = 0;
        List<VisItem> items = new();
        
        foreach (var record in records)
        {
            StringBuilder content = new();

            content.AppendLine($"<b>{record.Headline}</b><br />");
            foreach(var tdesc in record.DescribeTemporality())
                content.AppendLine($"<cite>{tdesc}</cite><br />");

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
                Title = record.Headline,
                Type = record.StartDate == record.EndDate ? "point" : "range",
                Group = group?.Title ?? string.Empty,
                SubGroup = string.Empty,
                Content = content.ToString(),
                Start = record.StartDate,
                End = record.EndDate ?? DateOnlyExtensions.Today,
                Selectable = true
            };
            items.Add(item);
        }

        return Task.FromResult(items);
    }
}
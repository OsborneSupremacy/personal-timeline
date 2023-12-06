namespace Personal.Timeline.Web.Models.Vis;

public record VisTimeline
{
    public required List<VisGroup> Groups { get; init; }
    
    public required List<VisItem> Items { get; init; }
}
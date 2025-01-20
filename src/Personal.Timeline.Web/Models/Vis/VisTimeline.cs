namespace Personal.Timeline.Web.Models.Vis;

internal record VisTimeline
{
    public required List<VisGroup> Groups { get; init; }
    
    public required List<VisItem> Items { get; init; }
}
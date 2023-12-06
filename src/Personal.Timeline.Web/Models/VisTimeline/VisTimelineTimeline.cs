namespace Personal.Timeline.Web.Models.VisTimeline;

public record VisTimelineTimeline
{
    public required List<VisTimelineGroup> Groups { get; init; }
    
    public required List<VisTimelineItem> Items { get; init; }
}
namespace Personal.Timeline.Web.Models.Timeline3;

public record Timeline3Timeline
{
    public required TimelineTitle Title { get; init; }
    
    public required List<Era> Eras { get; init; }
    
    public required List<Event> Events { get; init; }
}
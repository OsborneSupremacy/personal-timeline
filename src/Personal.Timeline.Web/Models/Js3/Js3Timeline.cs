namespace Personal.Timeline.Web.Models.Js3;

internal record Js3Timeline
{
    public required Js3Title Title { get; init; }
    
    public required List<Js3Era> Eras { get; init; }
    
    public required List<Js3Event> Events { get; init; }
}
namespace Personal.Timeline.Web.Models.VisTimeline;

public record Group
{
    public required string Id { get; set; }
    
    public required string Title { get; set; }
    
    public string? ClassName { get; set; }
    
    public string? Style { get; set; }

    public required string Content { get; set; }
}
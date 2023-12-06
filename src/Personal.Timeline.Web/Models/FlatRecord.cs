namespace Personal.Timeline.Web.Models;

public record FlatRecord
{
    public string? Headline { get; init; }
    
    public string? Description1 { get; init; }
    
    public string? Description2 { get; init; }
    
    public string? Url { get; init; }
    
    public string? UrlDescription { get; init; }
    
    public DateTime StartDate { get; init; }
    
    public DateTime? EndDate { get; init; }
    
    public string? Group { get; init; }
}
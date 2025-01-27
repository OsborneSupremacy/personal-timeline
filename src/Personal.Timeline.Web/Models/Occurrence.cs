namespace Personal.Timeline.Web.Models;

public record Occurrence
{
    public required string Headline { get; init; }
    
    public string? Description1 { get; init; }
    
    public string? Description2 { get; init; }
    
    public string? Url { get; init; }
    
    public string? UrlDescription { get; init; }
    
    public required DateOnly StartDate { get; init; }
    
    public DateOnly? EndDate { get; init; }
    
    public required string Group { get; init; }
}
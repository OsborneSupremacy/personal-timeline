namespace Personal.Timeline.Web.Models.Vis;

public record VisItem
{
    public required string Id { get; init; }
    
    public required string Title { get; init; }
    
    /// <summary>
    /// Should be box (default), point, range, background.
    /// Box and point require a start date.
    /// Range requires a start and end date. Background requires no date.
    /// </summary>
    public required string Type { get; init; }
    
    public required string Group { get; init; }
    
    public required string SubGroup { get; init; }
    
    public string? ClassName { get; init; }
    
    public string? Style { get; init; }
    
    public string? Align { get; init; }
    
    public required string Content { get; init; }
    
    public required DateOnly Start { get; init; }
    
    public DateOnly? End { get; init; }
    
    public bool Selectable { get; init; }
}
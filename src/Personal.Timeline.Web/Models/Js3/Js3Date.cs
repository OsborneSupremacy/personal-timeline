namespace Personal.Timeline.Web.Models.Js3;

public record Js3Date
{
    public required string Year { get; init; }
    
    public required string Month { get; init; }
    
    public required string Day { get; init; }
}
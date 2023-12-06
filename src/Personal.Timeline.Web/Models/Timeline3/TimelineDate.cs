namespace Personal.Timeline.Web.Models.Timeline3;

public record TimelineDate
{
    public required string Year { get; init; }
    
    public required string Month { get; init; }
    
    public required string Day { get; init; }
}
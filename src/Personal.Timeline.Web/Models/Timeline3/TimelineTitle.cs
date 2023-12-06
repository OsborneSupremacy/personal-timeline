namespace Personal.Timeline.Web.Models.Timeline3;

public record TimelineTitle
{
    public TimelineMedia? Media { get; init; }
    
    public TimelineText? Text { get; init; }
}
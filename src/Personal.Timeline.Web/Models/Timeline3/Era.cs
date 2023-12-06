using System.Text.Json.Serialization;

namespace Personal.Timeline.Web.Models.Timeline3;

public record Era
{
    public TimelineText? Text { get; init; }
    
    [JsonPropertyName("start_date")]
    public required TimelineDate StartDate { get; init; }
    
    [JsonPropertyName("end_date")]
    public TimelineDate? EndDate { get; init; }
}
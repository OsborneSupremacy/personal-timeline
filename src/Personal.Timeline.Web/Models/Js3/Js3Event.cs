using System.Text.Json.Serialization;

namespace Personal.Timeline.Web.Models.Js3;

internal record Js3Event
{
    public required string Group { get; init; }
    
    public required Js3Text Text { get; init; }
    
    [JsonPropertyName("start_date")]
    public required Js3Date StartDate { get; init; }
    
    [JsonPropertyName("end_date")]
    public Js3Date? EndDate { get; init; }
}
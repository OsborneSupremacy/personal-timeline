namespace Personal.Timeline.Web.Models.Messaging;

public record TimelineRequest
{
    public required string BasePath { get; init; }
    public required List<SourceItem> Items { get; init; }
}
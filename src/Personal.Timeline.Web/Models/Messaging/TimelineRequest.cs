namespace Personal.Timeline.Web.Models.Messaging;

internal record TimelineRequest
{
    public required string BasePath { get; init; }
    public required List<Occurrence> Items { get; init; }
}
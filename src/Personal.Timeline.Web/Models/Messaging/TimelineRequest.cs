namespace Personal.Timeline.Web.Models.Messaging;

internal record TimelineRequest
{
    public required string BasePath { get; init; }
    public required List<Occurence> Items { get; init; }
}
namespace Personal.Timeline.Web.Models.Messaging;

public record WriteOutputRequest<T>
{
    public required T Data { get; init; }
    
    public required string BasePath { get; init; }
    
    public required string FileName { get; init; }
};
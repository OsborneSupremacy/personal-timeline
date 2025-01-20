namespace Personal.Timeline.Web.Abstractions;

internal interface ITimelineGenerator<T> where T: class
{
    public Task<T> GenerateAsync(TimelineRequest request);
    
    public Task WriteAsync(T data, string basePath);
}
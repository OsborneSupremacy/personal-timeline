namespace Personal.Timeline.Web.Abstractions;

public interface ITimelineGenerator<T>
{
    public Task<T> GenerateAsync(TimelineRequest request);
    
    public Task WriteAsync(T data, string basePath);
}
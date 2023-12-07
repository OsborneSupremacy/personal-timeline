namespace Personal.Timeline.Web.Abstractions;

public interface ITimelineGenerator<T> where T: class
{
    public Task<T> GenerateAsync(TimelineRequest request);
    
    public Task WriteAsync(T data, string basePath);
}
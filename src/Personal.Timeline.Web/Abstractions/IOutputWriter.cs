namespace Personal.Timeline.Web.Abstractions;

public interface IOutputWriter
{
    public Task WriteAsync<T>(
        WriteOutputRequest<T> request
    );
}
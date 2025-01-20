namespace Personal.Timeline.Web.Abstractions;

internal interface IOutputWriter
{
    public Task WriteAsync<T>(
        WriteOutputRequest<T> request
    );
}
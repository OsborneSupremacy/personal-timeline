using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Personal.Timeline.Web.Services;

internal class OutputWriterService : IOutputWriter
{
    private class LowerCaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name) =>
            name.ToLower();
    }
    
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        PropertyNamingPolicy = new LowerCaseNamingPolicy(),
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };
    
    public async Task WriteAsync<T>(
        WriteOutputRequest<T> request
    )
    {
        if(!Directory.Exists(request.BasePath))
            Directory.CreateDirectory(request.BasePath);
        
        await File.WriteAllTextAsync(
            Path.Combine(request.BasePath, request.FileName),
            JsonSerializer.Serialize(request.Data, Options)
        );
    }
}

namespace Personal.Timeline.Web.Services;

public class TimelineGeneratorService
{
    private readonly SourceReaderService _sourceReaderService;
    
    private readonly Js3TimelineService _js3TimelineGenerator;
    
    private readonly VisTimelineService _visTimelineGenerator;

    public TimelineGeneratorService(
        SourceReaderService sourceReaderService,
        Js3TimelineService js3TimelineGenerator,
        VisTimelineService visTimelineGenerator
        )
    {
        _sourceReaderService = sourceReaderService ?? throw new ArgumentNullException(nameof(sourceReaderService));
        _js3TimelineGenerator = js3TimelineGenerator ?? throw new ArgumentNullException(nameof(js3TimelineGenerator));
        _visTimelineGenerator = visTimelineGenerator ?? throw new ArgumentNullException(nameof(visTimelineGenerator));
    }

    public async Task GenerateAllAsync()
    {
        var basePath = ReflectionUtilities.GetBaseOutputPath();
        if(!Directory.Exists(basePath))
            Directory.CreateDirectory(basePath);

        var sourceItems = (await _sourceReaderService.ReadAllAsync()).ToList();
        
        await GenerateAsync(_js3TimelineGenerator, sourceItems, Path.Combine(basePath, "js3"));
        await GenerateAsync(_visTimelineGenerator, sourceItems, Path.Combine(basePath, "vis"));
    }

    private static async Task GenerateAsync<T>(
        ITimelineGenerator<T> generator,
        List<SourceItem> sourceItems,
        string basePath
        ) where T : class
    {
        var data = await generator.GenerateAsync(new TimelineRequest
        {
            BasePath = basePath,
            Items = sourceItems
        });

        await generator.WriteAsync(data, basePath);
    }
}
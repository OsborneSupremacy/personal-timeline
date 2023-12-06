using Personal.Timeline.Web.Models.Js3;

namespace Personal.Timeline.Web.Services;

public class TimelineGeneratorService
{
    private readonly SourceReaderService _sourceReaderService;
    
    private readonly ITimelineGenerator<Js3Timeline> _js3TimelineGenerator;
    
    private readonly ITimelineGenerator<VisTimelineService> _visTimelineGenerator;

    public TimelineGeneratorService(
        SourceReaderService sourceReaderService,
        ITimelineGenerator<Js3Timeline> js3TimelineGenerator,
        ITimelineGenerator<VisTimelineService> visTimelineGenerator
        )
    {
        _sourceReaderService = sourceReaderService ?? throw new ArgumentNullException(nameof(sourceReaderService));
        _js3TimelineGenerator = js3TimelineGenerator ?? throw new ArgumentNullException(nameof(js3TimelineGenerator));
        _visTimelineGenerator = visTimelineGenerator ?? throw new ArgumentNullException(nameof(visTimelineGenerator));
    }

    public async Task GenerateAllAsync()
    {
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "output");
        if(!Directory.Exists(basePath))
            Directory.CreateDirectory(basePath);

        var sourceItems = (await _sourceReaderService.ReadAllAsync()).ToList();
        
        await _js3TimelineGenerator.GenerateAsync(new TimelineRequest
        {
            BasePath = Path.Combine(basePath, "js3"),
            Items = sourceItems
        });
        
        await _visTimelineGenerator.GenerateAsync(new TimelineRequest
        {
            BasePath = Path.Combine(basePath, "vis"),
            Items = sourceItems
        });
    }
}
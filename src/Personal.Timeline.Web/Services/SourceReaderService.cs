using ClosedXML.Excel;

namespace Personal.Timeline.Web.Services;

internal class SourceReaderService
{
    private readonly IConfiguration _configuration;
    
    public SourceReaderService(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public Task<List<Occurence>> ReadAllAsync()
    {
        var sourcePath = _configuration["SourceExcelPath"];
        
        using var workbook = new XLWorkbook(sourcePath);

        var worksheet = workbook.Worksheets.Single();

        var items = worksheet.RowsUsed()
            .Skip(1)
            .Select(row => new Occurence
            {
                Headline = row.Cell(1).GetString(),
                Description1 = row.Cell(2).GetString(),
                Description2 = row.Cell(3).GetString(),
                Url = row.Cell(4).GetString(),
                UrlDescription = row.Cell(5).GetString(),
                StartDate = DateOnly.FromDateTime(row.Cell(6).GetDateTime()),
                EndDate = string.IsNullOrEmpty(row.Cell(7).GetString()) ? null : DateOnly.FromDateTime(row.Cell(7).GetDateTime()),
                Group = row.Cell(8).GetString(),
            });

        return Task.FromResult(items.ToList());
    }
}
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using PriceAnalyzer.Dto;
using System.Globalization;

namespace PriceAnalyzer.Controllers;

[ApiController]
public class CSVController : ControllerBase
{
    [HttpPost("/csv")]
    [ProducesResponseType(typeof(ParseResponse), 200)]
    public async Task<IActionResult> Parse([FromBody] ParseResponse request)
    {
        const string fileName = "data.csv";

        var configuration = new CsvConfiguration(new CultureInfo("ru-RU"))
        {
            Delimiter = ";"
        };

        Response.Headers["Content-Disposition"] = $"attachment; filename={fileName}";

        var memoryStream = new MemoryStream();

        await using var streamWriter = new StreamWriter(memoryStream, leaveOpen: true);
        await using var csvWriter = new CsvWriter(streamWriter, configuration);

        await csvWriter.WriteRecordsAsync(request.Advertisements);
        await csvWriter.FlushAsync();

        memoryStream.Seek(0, SeekOrigin.Begin);

        return new FileStreamResult(memoryStream, "application/csv") { FileDownloadName = fileName };
    }
}
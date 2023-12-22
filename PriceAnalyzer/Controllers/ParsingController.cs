using AvitoParser;
using Microsoft.AspNetCore.Mvc;
using PriceAnalyzer.Dto;
using PriceAnalyzer.Logic;

namespace PriceAnalyzer.Controllers;

[ApiController]
public class ParsingController : ControllerBase
{
    [HttpGet("/parse")]
    public async Task<IActionResult> Parse(
        [FromQuery] ParseRequest request,
        [FromServices] ParsingClient client)
    {
        var parser = new Parser(client.Client);
        var adverts = await parser.GetAdvertisements(request.Url, request.Amount!.Value);
        adverts.FillPriceDeviation();

        return new JsonResult(adverts);
    }
}
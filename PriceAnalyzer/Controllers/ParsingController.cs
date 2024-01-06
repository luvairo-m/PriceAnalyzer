using AvitoParser;
using Microsoft.AspNetCore.Mvc;
using PriceAnalyzer.Dto;
using PriceAnalyzer.Filters;

namespace PriceAnalyzer.Controllers;

[ApiController]
public class ParsingController : ControllerBase
{
    [HttpGet("/parse")]
    [ProducesResponseType(typeof(ParseResponse), 200)]
    [ParseResultFilter]
    public async Task<IActionResult> Parse(
        [FromQuery] ParseRequest request,
        [FromServices] ParsingClient client)
    {
        var parser = new Parser(client.Client);
        var adverts = await parser.GetAdvertisementsAsync(request.Url, request.Amount!.Value);
        return new JsonResult(new ParseResponse { Advertisements = adverts });
    }
}
using AvitoParser;
using Microsoft.AspNetCore.Mvc;
using PriceAnalyzer.Dto;
using PriceAnalyzer.Logic;

namespace PriceAnalyzer.Controllers;

[ApiController]
public class ParsingController : ControllerBase
{
    [HttpGet("/parse")]
    [ProducesResponseType(typeof(ParseResponse), 200)]
    public async Task<IActionResult> Parse(
        [FromQuery] ParseRequest request,
        [FromServices] ParsingClient client)
    {
        var parser = new Parser(client.Client);

        var adverts = await parser.GetAdvertisements(request.Url, request.Amount!.Value);
        var averagePrice = adverts.FillPriceDeviation();

        return new JsonResult(new ParseResponse(averagePrice, adverts));
    }
}
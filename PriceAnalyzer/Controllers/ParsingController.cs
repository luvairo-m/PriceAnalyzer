using AvitoParser;
using Microsoft.AspNetCore.Mvc;
using PriceAnalyzer.Dto;
using PriceAnalyzer.Filters;
using PriceAnalyzer.Logic;

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
        var medianPrice = adverts.GetMedianPrice();

        adverts.FillPriceDeviation(medianPrice);

        return new JsonResult(new ParseResponse(medianPrice, adverts));
    }
}
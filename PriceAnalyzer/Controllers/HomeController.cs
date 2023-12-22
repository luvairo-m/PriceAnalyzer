using AvitoParser;
using Microsoft.AspNetCore.Mvc;
using PriceAnalyzer.Dto;

namespace PriceAnalyzer.Controllers;

[ApiController]
public class HomeController : ControllerBase
{
    [HttpGet("/[action]")]
    public async Task<IActionResult> Parse(
        [FromQuery] ParseRequest request, 
        [FromServices] IHttpClientFactory factory)
    {
        var parser = new Parser(factory.CreateClient());
        var adverts = await parser.GetAdvertisements(request.Url, request.Amount!.Value);
        return new JsonResult(adverts);
    }
}
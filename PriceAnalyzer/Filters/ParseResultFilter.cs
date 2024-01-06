using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PriceAnalyzer.Dto;

namespace PriceAnalyzer.Filters;

public class ParseResultFilter : Attribute, IAsyncResultFilter
{
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        var response = (context.Result as JsonResult)!.Value as ParseResponse;
        var adverts = response!.Advertisements;

        await next();
    }
}
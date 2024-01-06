using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PriceAnalyzer.Dto;
using PriceAnalyzer.Fillers;

namespace PriceAnalyzer.Filters;

public class ParseResultFilter : Attribute, IAsyncResultFilter
{
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        FillerAggregator.ApplyFillers(((context.Result as JsonResult)!.Value as ParseResponse)!);
        await next();
    }
}
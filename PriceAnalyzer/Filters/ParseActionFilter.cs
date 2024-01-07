using AvitoParser;
using Microsoft.AspNetCore.Mvc.Filters;
using PriceAnalyzer.Dto;

namespace PriceAnalyzer.Filters;

public class ParseActionFilter : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var request = context.ActionArguments["request"]! as ParseRequest;

        if (new Uri(request!.Url).Host != Configuration.Host)
            throw new ArgumentException("Incorrect hostname");

        await next();
    }
}
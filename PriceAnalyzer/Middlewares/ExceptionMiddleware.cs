using PriceAnalyzer.Dto;

namespace PriceAnalyzer.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            context.Response.ContentType = "application/json";
            var exceptionResponse = GetExceptionResponse(exception);
            await context.Response.WriteAsJsonAsync(exceptionResponse);
        }
    }

    private static ExceptionResponse GetExceptionResponse(Exception exception)
    {
        var statusCode = StatusCodes.Status500InternalServerError;
        var message = exception.Message;

        statusCode = exception switch
        {
            InvalidOperationException => StatusCodes.Status404NotFound,
            HttpRequestException requestException => (int)requestException.StatusCode!,
            _ => statusCode
        };

        return new ExceptionResponse(message, statusCode);
    }
}
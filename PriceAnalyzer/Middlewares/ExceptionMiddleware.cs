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
            var exceptionResponse = GetErrorResponseObject(exception);
            await context.Response.WriteAsJsonAsync(exceptionResponse);
        }
    }

    private static ExceptionResponse GetErrorResponseObject(Exception exception)
    {
        string message;
        int statusCode;

        switch (exception)
        {
            case InvalidOperationException:
                message = "Bad url";
                statusCode = 404;
                break;
            case HttpRequestException:
                message = "Avito not available or too many requests";
                statusCode = 403;
                break;
            case ArgumentException:
                message = "Incorrect host";
                statusCode = 404;
                break;
            default:
                message = exception.Message;
                statusCode = 500;
                break;
        }

        return new ExceptionResponse(message, statusCode);
    }
}
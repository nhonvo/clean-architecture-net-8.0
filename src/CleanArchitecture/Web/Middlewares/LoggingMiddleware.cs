using System.Text.Json;
using CleanArchitecture.Application.Common.Utilities;

namespace CleanArchitecture.Web.Middlewares;
public class LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<LoggingMiddleware> _logger = logger;
    private const long MaxLogContentLength = 10_000_000; // 10 MB

    public async Task InvokeAsync(HttpContext context)
    {
        context.Request.EnableBuffering();
        if (await IsApiRequest(context))
        {
            await LogRequest(context);
            await LogResponse(context);
        }
        else
        {
            await _next(context);
        }
    }

    private async Task<bool> IsApiRequest(HttpContext context)
        => context.Request.Path.Value.StartsWith("/api", StringComparison.OrdinalIgnoreCase)
            || context.Request.Headers.Accept.Any(h => h.Contains("application/json", StringComparison.OrdinalIgnoreCase));

    private async Task LogRequest(HttpContext context)
    {
        using var memStream = new MemoryStream();
        await context.Request.Body.CopyToAsync(memStream);
        memStream.Seek(0, SeekOrigin.Begin);
        context.Request.Body.Position = 0; // Reset stream position
        var requestBody = await new StreamReader(memStream).ReadToEndAsync();
        LogHelper.LogRequest(_logger, context.Request.Path, JsonSerializer.Serialize(requestBody));
    }

    private async Task LogResponse(HttpContext context)
    {
        var originalBodyStream = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        try
        {
            await _next(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBodyText = await new StreamReader(responseBody).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            if (responseBodyText.Length <= MaxLogContentLength)
            {
                LogHelper.LogResponse(_logger, "Middleware Log Response", responseBodyText, context.Response.StatusCode);
            }
            else
            {
                LogHelper.LogResponse(_logger, "Middleware Log Response", $"Response body too large to log. ContentLength: {responseBodyText.Length} bytes", context.Response.StatusCode);
            }

            await responseBody.CopyToAsync(originalBodyStream);
        }
        finally
        {
            context.Response.Body = originalBodyStream;
        }
    }
}

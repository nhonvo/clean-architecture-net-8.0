using System.Text.Json;
using CleanArchitecture.Application.Common.Utilities;

namespace CleanArchitecture.Web.Middlewares;

public class LoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger _logger = loggerFactory.CreateLogger<LoggingMiddleware>();

    public async Task InvokeAsync(HttpContext context)
    {
        bool isValidFormatRequest = await LogRequest(context);
        if (!isValidFormatRequest)
            await _next(context);
        else
            await LogResponse(context);
    }

    private async Task<bool> LogRequest(HttpContext context)
    {
        context.Request.EnableBuffering();
        var isMultipart = context.Request.HasFormContentType;

        if (isMultipart)
        {
            var form = context.Request.Form;
            foreach (var file in form.Files)
            {
                ExecuteLogRequest("File Upload", logString: $"File: {file.FileName}, Size: {file.Length} bytes");
            }
            return true;
        }
        else
        {
            using MemoryStream memStream = new MemoryStream();
            context.Request.Body.Position = 0;
            await context.Request.Body.CopyToAsync(memStream);
            memStream.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(memStream);
            var requestAsText = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;

            try
            {
                ExecuteLogRequest("Request", logString: JsonSerializer.Serialize(requestAsText));
            }
            catch (Exception exception)
            {
                ExecuteLogRequest("Request", logString: requestAsText.Replace(Environment.NewLine, string.Empty));
                _logger.LogError("Exception: {exceptionMessage}", exception);
                return false;
            }
            return true;
        }
    }

    private void ExecuteLogRequest(string path, string logString)
    {
        LogHelper.LogRequest(_logger, path, logString);
    }

    private async Task LogResponse(HttpContext context)
    {
        // Store the original response stream
        var originalBodyStream = context.Response.Body;

        // Create a new memory stream to capture the response
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        try
        {
            // Call the next middleware in the pipeline
            await _next(context);

            // Reset the position to the beginning of the stream to read it
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseAsText = await new StreamReader(responseBody).ReadToEndAsync();

            // Check if the response content is JSON and attempt to deserialize it
            if (context.Response.ContentType != null && context.Response.ContentType.Contains("application/json"))
            {
                try
                {
                    var response = JsonSerializer.Deserialize<object>(responseAsText);
                    LogHelper.LogResponse(
                        _logger,
                        "Response",
                        JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true }),
                        context.Response.StatusCode
                    );
                }
                catch (JsonException)
                {
                    // If deserialization fails, log as raw text
                    LogHelper.LogResponse(
                        _logger,
                        "Response",
                        responseAsText,
                        context.Response.StatusCode
                    );
                }
            }
            else
            {
                // Log as plain text if the content type is not JSON
                LogHelper.LogResponse(
                    _logger,
                    "Response",
                    responseAsText,
                    context.Response.StatusCode
                );
            }

            // Reset the position of the memory stream and copy to the original response stream
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBodyStream);
        }
        finally
        {
            // Restore the original response body stream
            context.Response.Body = originalBodyStream;
        }
    }
}

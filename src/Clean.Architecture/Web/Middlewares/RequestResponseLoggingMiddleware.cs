using Clean.Architecture.Application.Common;
using Clean.Architecture.Application.Common.Utilities;
using Newtonsoft.Json;

namespace Clean.Architecture.Web.Middlewares
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly AppSettings _appSettings;
        public RequestResponseLoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, AppSettings appSettings)
        {
            _next = next;
            _logger = loggerFactory
                      .CreateLogger<RequestResponseLoggingMiddleware>();
            _appSettings = appSettings;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (_appSettings.Logging.RequestResponse.IsEnabled)
            {
                bool IsValidFormatRequest = await LogRequest(context);
                // return false if request json is wrong format
                // return true if request json is correct format
                // run next middleware and ignore response log if request json is wrong format
                if (!IsValidFormatRequest)
                    await _next(context);
                else
                    await LogResponse(context);
            }
            else
            {
                await _next(context);
            }
        }

        private async Task<bool> LogRequest(HttpContext context)
        {
            context.Request.EnableBuffering();
            using MemoryStream memStream = new MemoryStream();
            context.Request.Body.Position = 0;
            await context.Request.Body.CopyToAsync(memStream);
            memStream.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(memStream);
            var requestAsText = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;
            var path = context.Request.Path.ToString();

            try
            {
                ExecuteLogRequest("Request", logString: JsonConvert.SerializeObject(JsonConvert.DeserializeObject(requestAsText)));
            }
            catch (Exception exception)
            {
                ExecuteLogRequest("Request", logString: requestAsText.Replace(System.Environment.NewLine, string.Empty));
                _logger.LogError($"Exception:{exception}");
                return false;
            }

            return true;
        }

        private void ExecuteLogRequest(string path, string logString)
        {
            LogHelper.LogRequest(_logger, path, logString, _appSettings.Logging.RequestResponse.IsEnabled);
        }

        private async Task LogResponse(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;
            await _next(context);
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(context.Response.Body);
            var responseAsText = await reader.ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var path = context.Request.Path.ToString();

            LogHelper.LogResponse(
                _logger, "Response",
                JsonConvert.SerializeObject(responseAsText),
                context.Response.StatusCode,
                _appSettings.Logging.RequestResponse.IsEnabled
                );

            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
}
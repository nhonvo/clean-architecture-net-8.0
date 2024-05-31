namespace CleanArchitecture.Application.Common.Utilities;

public static class LogHelper
{
    public static void LogRequest(ILogger logger, string path, string requestData, bool isEnabled)
    {
        if (isEnabled)
        {
            logger.LogInformation(
                "Request Path: {path}\nRequest Data: {requestData}",
                path,
                requestData
            );
        }
    }

    public static void LogResponse(ILogger logger, string logMessage, string responseData, int statusCode, bool isEnabled)
    {
        if (isEnabled)
        {
            logger.LogInformation(
                "{logMessage}\nResponse Data: {responseData}\nStatus Code: {statusCode}",
                logMessage,
                responseData,
                statusCode
            );
        }

    }
}

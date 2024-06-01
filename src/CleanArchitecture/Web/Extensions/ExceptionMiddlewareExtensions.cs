using System.Net;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Domain.Constants;
using Microsoft.AspNetCore.Diagnostics;

namespace CleanArchitecture.Web.Extensions;
public static class ExceptionMiddlewareExtensions
{
    public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILogger logger)
    {

        app.UseExceptionHandler(new ExceptionHandlerOptions
        {
            AllowStatusCode404Response = true,
            ExceptionHandler = async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var errorId = Guid.NewGuid();

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    string errorMessage = string.Empty;
                    string errorCode = string.Empty;

                    if (contextFeature.Error is UserFriendlyException userFriendlyException)
                    {
                        switch (userFriendlyException.ErrorCode)
                        {
                            case ErrorCode.NotFound:
                                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                                errorMessage = userFriendlyException.UserFriendlyMessage;
                                errorCode = $"{ApplicationConstants.Name}.{ErrorRespondCode.NOT_FOUND}";
                                break;
                            case ErrorCode.VersionConflict:
                                context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                                errorMessage = userFriendlyException.UserFriendlyMessage;
                                errorCode = $"{ApplicationConstants.Name}.{ErrorRespondCode.VERSION_CONFLICT}";
                                break;
                            case ErrorCode.ItemAlreadyExists:
                                context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                                errorMessage = userFriendlyException.UserFriendlyMessage;

                                errorCode = $"{ApplicationConstants.Name}.{ErrorRespondCode.ITEM_ALREADY_EXISTS}";
                                break;
                            case ErrorCode.Conflict:
                                context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                                errorMessage = userFriendlyException.UserFriendlyMessage;

                                errorCode = $"{ApplicationConstants.Name}.{ErrorRespondCode.CONFLICT}";
                                break;
                            case ErrorCode.BadRequest:
                                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                                errorMessage = userFriendlyException.UserFriendlyMessage;
                                errorCode = $"{ApplicationConstants.Name}.{ErrorRespondCode.BAD_REQUEST}";
                                break;
                            case ErrorCode.Unauthorized:
                                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                                errorMessage = userFriendlyException.UserFriendlyMessage;
                                errorCode = $"{ApplicationConstants.Name}.{ErrorRespondCode.UNAUTHORIZED}";
                                break;
                            case ErrorCode.Internal:
                                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                errorMessage = userFriendlyException.UserFriendlyMessage;
                                errorCode = $"{ApplicationConstants.Name}.{ErrorRespondCode.INTERNAL_ERROR}";
                                break;
                            case ErrorCode.UnprocessableEntity:
                                context.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                                errorMessage = userFriendlyException.UserFriendlyMessage;
                                errorCode = $"{ApplicationConstants.Name}.{ErrorRespondCode.UNPROCESSABLE_ENTITY}";
                                break;
                            default:
                                context.Response.StatusCode = 500;
                                errorMessage = userFriendlyException.UserFriendlyMessage;
                                errorCode = $"{ApplicationConstants.Name}.{ErrorRespondCode.GENERAL_ERROR}";
                                break;
                        }
                    }
                    else
                    {
                        context.Response.StatusCode = 500;
                        errorCode = $"{ApplicationConstants.Name}.{ErrorRespondCode.GENERAL_ERROR}";
                        errorMessage = "An error has occurred.";
                    }
                    await context.Response.WriteAsync($@"
                                {{
                                    ""errors"":[
                                        {{
                                            ""code"":""{errorCode}"",
                                            ""message"":""{errorMessage}, ErrorId:{errorId}""
                                        }}
                                    ]
                                }}");

                    logger.LogError($"ErrorId:{errorId} Exception:{contextFeature.Error}");
                }
            }
        });
    }
}

using System.Diagnostics.CodeAnalysis;
using CleanArchitecture.Domain.Constants;

namespace CleanArchitecture.Application.Common.Exceptions;

[ExcludeFromCodeCoverage]
public static class UserException
{
    public static UserFriendlyException UserAlreadyExistsException(string field)
        => new(ErrorCode.BadRequest, string.Format(UserErrorMessage.AlreadyExists, field), string.Format(UserErrorMessage.AlreadyExists, field));

    public static UserFriendlyException UserUnauthorizedException()
        => new(ErrorCode.Unauthorized, UserErrorMessage.Unauthorized, UserErrorMessage.Unauthorized);

    public static UserFriendlyException InternalException(Exception? exception)
        => new(ErrorCode.Internal, ErrorMessage.InternalError, ErrorMessage.InternalError, exception);

    public static UserFriendlyException BadRequestException(string errorMessage)
        => new(ErrorCode.BadRequest, errorMessage, errorMessage);
}
